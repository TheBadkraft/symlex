
using System.Collections.Concurrent;

using Synaptic.Core;
using Synaptic.Analysis;
using Synaptic.Threading;

namespace Synaptic.Services.Analysis;

/// <summary>
/// A service that parses tokens.
/// </summary>
public partial class ParsingService : IObserver<IStateChange>, IParsingService
{
    private const string AGENT_ID = "InputAgent";

    private ConcurrentQueue<IReadOnlyList<Token>> InputQueue { get; init; } = new();

    private AutoResetEvent InputSignal { get; init; } = new(false);
    private ParserFactory Factory { get; init; }
    private ITaskingService Tasking { get; init; }
    private InputParser Parser { get; init; }
    private IDisposable Subscriber { get; set; }

    internal ParsingService(ITaskingService taskingService)
    {
        Factory = new(Tasking = taskingService);
        Parser = new();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Enqueue<TData>(TData input)
    {
        if (input is IReadOnlyList<Token> tokens)
        {
            InputQueue.Enqueue(tokens);
            //  trigger input dequeueing
            InputSignal.Set();
        }
        else
        {
            //  TODO: replace with error reporting and recover
            throw new ArgumentException("Invalid input type.");
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void OnNext(IStateChange change)
    {
        if (change.CheckState(state => state.Is(SynapticState.Shutdown)))
        {
            Stop();
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void OnRegistered(IServiceContainer serviceContainer)
    {
        Subscriber = serviceContainer.GetService<IStateOverwatch>().Subscribe(this);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Start()
    {
        //  create the task agent
        if (!Tasking.CreateTaskAgent(AGENT_ID, MonitorInputQueue))
        {
            //  TODO: replace with error reporting and shut down
            throw new InvalidOperationException($"Task Agent {AGENT_ID} could not be created.");
        }
        //  start monitoring input queue
        Tasking.StartAgent(AGENT_ID);
        //  start factory
        Factory.Start();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>
    /// This method will stop the input agent and clear the input queue. 
    /// The input signal will be set to allow the monitor to exit.
    /// </remarks>
    public void Stop()
    {
        //  clear the input queue
        InputQueue.Clear();
        //  recall the input agent
        Tasking.RecallAgent(AGENT_ID);
        //  set the input signal
        InputSignal.Set();
        //  stop the factory
        Factory.Stop();
        //  unsubscribe from the state change
        Subscriber.Dispose();
    }

    /// <remarks>
    /// The monitor method waits for input to be enqueued and then loops to
    /// dequeue and parse the input.The input parser only parses the first
    /// few tokens to determine the context of the input.
    /// 
    /// The context descriptor is then passed to the factory for analysis.
    /// 
    /// The InputSignal is an auto reset event. The monitor loops until the
    /// queue is empty, then waits for a signal that input has been enqueued.
    ///
    /// If the task agent or parsing service is being shut down, we need to
    /// determine the status of the agent. Just because a cancellation request 
    /// is initiated does not mean it will stop. It could be waiting at the
    /// input signal.
    /// </remarks>
    private void MonitorInputQueue(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //  we wait here rather cycling an empty queue
            InputSignal.WaitOne();

            //  dequeue until empty
            while (InputQueue.TryDequeue(out var input))
            {
                //  parse the input - this is a blocking operation
                Parser.ParseInput(input);
                //  enqueue the descriptor - this parser is done
                Factory.Enqueue(Parser.Descriptor);

                //  check if cancellation is requested
                if (cancellationToken.IsCancellationRequested) break;
            }
        }
    }
}

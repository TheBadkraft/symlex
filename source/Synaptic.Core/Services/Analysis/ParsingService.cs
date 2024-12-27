
using System.Collections.Concurrent;

using Synaptic.Core;
using Synaptic.Analysis;

namespace Synaptic.Services.Analysis;

/// <summary>
/// A service that parses tokens.
/// </summary>
public partial class ParsingService : IObserver<IStateChange>, IParsingService
{
    private ConcurrentQueue<Data> InputQueue { get; init; } = new();

    private ParserFactory Factory { get; init; }

    internal ParsingService()
    {
        Factory = new();
    }

    //  threaded queue: input data and enqueue response descriptor
    public void Enqueue<TData>(TData input)
    {
        if (input is Data data)
        {
            InputQueue.Enqueue(data);
        }
        else
        {
            //  TODO: replace with error reporting and recover
            throw new ArgumentException("Invalid input type.");
        }
    }
    private void Run()
    {
        //  TODO: threaded run monitor

    }

    public void OnNext(IStateChange value)
    {
        throw new NotImplementedException();
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnRegistered(IServiceContainer serviceContainer)
    {
        serviceContainer.GetService<IStateOverwatch>().Subscribe(this);
    }
}

using System.Collections.Concurrent;

using Synaptic.Core;
using Synaptic.Analysis;
using Synaptic.Threading;

namespace Synaptic.Services.Analysis;

public partial class ParsingService
{
    //  private class ParserFactory
    private class ParserFactory
    {
        private const string AGENT_ID = "AnalysisAgent";

        private ConcurrentQueue<ContextDescriptor> AnalysisQueue { get; init; } = new();
        private AutoResetEvent AnalysisSignal { get; init; } = new(false);
        private ITaskingService Tasking { get; init; }

        /*
            The parser factory will be provided the results of the input parsing
            operation and will return a parser that can be used to process the
            input data. The factory will be responsible for selecting the correct
            parser based on the context descriptor provided.
        */
        internal ParserFactory(ITaskingService taskingService) => Tasking = taskingService;

        //  Start the analysis agent
        internal void Start()
        {
            //AnalysisAgent = Tasking.CreateTaskAgent(ANALYSIS_AGENT, MonitorAnalysisQueue);
            //  create the tasking agent
            if (!Tasking.CreateTaskAgent(AGENT_ID, MonitorAnalysisQueue))
            {
                throw new InvalidOperationException($"Task Agent {AGENT_ID} could not be created..");
            }
            //  start monitoring the analysis queue
            Tasking.StartAgent(AGENT_ID);
        }
        //  Stop the analysis agent
        internal void Stop()
        {
            //  clear the analysis queue
            AnalysisQueue.Clear();
            //  recall the analysis agent
            Tasking.RecallAgent(AGENT_ID);
            //  set the analysis signal
            AnalysisSignal.Set();
        }

        internal void Enqueue(ContextDescriptor contextDescriptor)
        {
            AnalysisQueue.Enqueue(contextDescriptor);
            //  trigger analysis dequeueing
            AnalysisSignal.Set();
        }

        private void MonitorAnalysisQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //  we actually wait here rather cycling an empty queue
                AnalysisSignal.WaitOne();

                //  dequeue until empty
                while (AnalysisQueue.TryDequeue(out var descriptor))
                {
                    //  borrow the terminal to output the context descriptor
                    var terminal = SynapticHub.Instance.Resources.GetResource<IO.ITerminal>();
                    terminal.Prompt($"Context: {descriptor.Data}");
                }
            }
        }
    }
}

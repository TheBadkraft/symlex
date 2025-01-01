
namespace Synaptic.Threading;

public class TaskAgent : IDisposable
{
    private Task Task { get; set; }
    private CancellationTokenSource TaskToken { get; set; }
    private Action<CancellationToken> TaskAction { get; init; }

    internal TaskAgent(Action<CancellationToken> action)
    {
        TaskAction = action;
        TaskToken = new();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Start()
    {
        Task = Task.Run(() =>
        {
            try
            {
                TaskAction(TaskToken.Token);
            }
            catch (OperationCanceledException)
            {
                //  TODO: handle cancellation
            }
        }, TaskToken.Token);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Stop()
    {
        TaskToken.Cancel();
        try
        {
            Task.Wait();
        }
        catch (AggregateException ae) when (ae.InnerException is OperationCanceledException)
        {
            //  Expected when task cancellation is requested
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        Stop();
        TaskToken.Dispose();
    }
}

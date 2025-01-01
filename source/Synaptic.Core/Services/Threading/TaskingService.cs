
using Synaptic.Core;
using Synaptic.Threading;

namespace Synaptic.Services.Threading;

public class TaskingService : ITaskingService
{
    private const int MIN_THREADS = 5;
    private const int MAX_THREADS = 10;

    private static Dictionary<string, TaskAgent> Agents { get; } = new();
    private bool IsRunning { get; set; }

    internal TaskingService() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="id"><inheritdoc/></param>
    /// <param name="action"><inheritdoc/></param>
    /// <returns><inheritdoc/></returns>
    public bool CreateTaskAgent(string id, Action<CancellationToken> action)
    {
        if (!IsRunning)
        {
            //  TODO: replace with error reporting and recover
            throw new InvalidOperationException("Tasking Service is not available");
        }
        if (!Agents.TryAdd(id, new(action)))
        {
            //  TODO: replace with error reporting and recover
            throw new ArgumentException($"Task Key {id} is already registered.");
        }

        return Agents.ContainsKey(id);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void OnRegistered(IServiceContainer serviceContainer)
    {
        //  set threadpool thread limits
        ThreadPool.SetMinThreads(MIN_THREADS, MIN_THREADS);
        ThreadPool.SetMaxThreads(MAX_THREADS, MAX_THREADS);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Start()
    {
        IsRunning = true;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Stop()
    {
        //  TODO: recall all task agents and dispose
        if (ThreadPool.PendingWorkItemCount > 0)
        {
            // do we wait for it or what ... ???
        }
        Shutdown();
        IsRunning = false;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="id"><inheritdoc/></param>
    public bool StartAgent(string id)
    {
        if (IsRunning && Agents.TryGetValue(id, out var agent))
        {
            agent.Start();
            return true;
        }
        return false;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="id"><inheritdoc/></param>
    public void RecallAgent(string id)
    {
        if (Agents.TryGetValue(id, out var agent))
        {
            RecallAgent(agent);
            Agents.Remove(id);
        }
    }

    //  shutdown will recall task agents
    private void Shutdown()
    {
        var agentIds = Agents.Keys.AsEnumerable();
        foreach (var id in agentIds)
        {
            RecallAgent(Agents[id]);
            Agents.Remove(id);
        }
    }

    private void RecallAgent(TaskAgent agent)
    {
        agent.Stop();
        agent.Dispose();
    }
}

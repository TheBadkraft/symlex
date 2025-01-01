
namespace Synaptic.Core;

/// <summary>
/// Represents a service for managing threads and the threadpool
/// </summary>
public interface ITaskingService : ISynapticService
{
    /// <summary>
    /// Create and register a task agent
    /// </summary>
    /// <param name="agentId">Agent identifier</param>
    /// <param name="taskAction">Agent task action</param>
    /// <returns>TRUE if the agent was created successfully; otherwise FALSE</returns>
    bool CreateTaskAgent(string agentId, Action<CancellationToken> taskAction);
    /// <summary>
    /// Starts the task agent
    /// </summary>
    /// <param name="agentId">Agent identifier</param>
    /// <returns>TRUE if the agent was started successfully; otherwise FALSE</returns>
    bool StartAgent(string agentId);
    /// <summary>
    /// Recalls the task agent to the tasking service
    /// </summary>
    /// <param name="agentId">Agent identifier</param>
    void RecallAgent(string agentId);
}
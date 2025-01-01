// Last update: 2024-12-16 14:30

using Synaptic.IO;

namespace Synaptic.Core;

/// <summary>
/// The main runtime of the Synaptic interpreter.
/// </summary>
public partial class Runtime : IRuntime
{
    const string PROMPT = "E: ";

    private InputHandler InputHandler { get; set; }
    private SynapticTerminal Terminal { get; init; }
    private RuntimeState State
    {
        get => SynapticHub.Instance.GetState(state => state.GetState<RuntimeState>());
        set => SynapticHub.Instance.UpdateState(state => state.SetState(value));
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    ITerminal IRuntime.Terminal => Terminal;

    /// <summary>
    /// Initializes a new instance of the <see cref="Runtime"/> class.
    /// </summary>
    /// <param name="dataFilePath">The path to the data file.</param>
    public Runtime()
    {
        //  initialize the terminal first
        Terminal = new SynapticTerminal();
        //  on runtime initialization, register self with SynapticHub as the Runtime
        SynapticHub.Instance.RegisterRuntime(this);
        //  set the runtime state to idle
        State = RuntimeState.Idle;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Initialize(IServiceContainer services, IResourceService resources)
    {
        //  register the terminal
        _ = resources.RegisterResource<ITerminal>(Terminal);
        //  register the input buffer
        var InputBuffer = resources.RegisterResource<IInputBuffer>(new InputBuffer());
        InputHandler = new(services, resources) { Runtime = this };
    }
    /// <summary>
    /// Launches the Synaptic interpreter runtime.
    /// </summary>
    public void Launch()
    {
        var state = State = RuntimeState.Running;

        while (state == RuntimeState.Running)
        {
            InputHandler.Process();
            state = State;
            if (state == RuntimeState.Running || state == RuntimeState.Idle) Terminal.Prompt();
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void RequestShutdown()
    {
        //  set shutdown state
        State = RuntimeState.Shutdown;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        Terminal.Dispose();
    }
}

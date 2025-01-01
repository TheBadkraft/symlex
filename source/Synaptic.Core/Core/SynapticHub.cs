
using System.Runtime.CompilerServices;

using Synaptic.Services.Analysis;
using Synaptic.Services.Threading;

[assembly: InternalsVisibleTo("terminal.tests")]

namespace Synaptic.Core;

/// <summary>
/// The core of the Synaptic Runtime. This class is responsible for retaining 
/// the state of the system and notifying observers of changes.
/// </summary>
public partial class SynapticHub : ISynapticHub
{
    private static readonly Lazy<SynapticHub> INSTANCE = new(() => new());

    private IRuntime Runtime { get; set; }
    private SystemState State { get; init; }
    private ServiceContainer Services { get; init; }
    private StateOverwatch Overwatch { get; init; }
    private TaskingService Tasking { get; init; }
    private ResourceManager Resources { get; init; }

    /// <summary>
    /// The singleton instance of the SynapticHub.
    /// </summary>
    public static ISynapticHub Instance => INSTANCE.Value;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    IServiceContainer ISynapticHub.Services => Services;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    IResourceService ISynapticHub.Resources => Resources;

    private SynapticHub()
    {
        Services = new();

        Overwatch = new(State = new());
        State.SetState(SynapticState.Idle);

        Tasking = new TaskingService();

        Services.RegisterService<IStateOverwatch>(Overwatch);
        Services.RegisterService<ITaskingService>(Tasking);
        Services.RegisterService<ILexerService>(new LexerService(Tasking));
        Services.RegisterService<IParsingService>(new ParsingService(Tasking));

        Resources = (ResourceManager)Services
            .RegisterService<IResourceService>(new ResourceManager());

        //  order matters
        Services.StartService<ITaskingService>();
        Services.StartService<ILexerService>();
        Services.StartService<IParsingService>();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="runtime">The runtime to register</param>
    public void RegisterRuntime(IRuntime runtime)
    {
        (Runtime = runtime).Initialize(Services, Resources);
        State.SetState(SynapticState.Running);
    }
}

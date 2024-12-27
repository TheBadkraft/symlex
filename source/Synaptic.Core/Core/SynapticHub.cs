
using System.Runtime.CompilerServices;
using Synaptic.Services.Analysis;

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

        Services.RegisterService<IStateOverwatch>(Overwatch);
        Services.RegisterService(new LexerService());
        Services.RegisterService<IParsingService>(new ParsingService());

        Resources = Services.RegisterService(new ResourceManager());

        // Services.GetService<IParsingService>().Run();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="runtime">The runtime to register</param>
    public void RegisterRuntime(IRuntime runtime)
    {
        (Runtime = runtime).Initialize(Services, Resources);
    }
}

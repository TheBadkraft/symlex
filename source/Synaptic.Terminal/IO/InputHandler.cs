
using System.Security;
using Synaptic.Analysis;
using Synaptic.Core;
using Synaptic.Services.Analysis;

namespace Synaptic.IO;

/// <summary>
/// Processes input from the terminal.
/// </summary>
public class InputHandler //  TODO: (IInputService : ISynapticService)
{
    private IInputBuffer InputBuffer { get; init; }
    private ILexerService Lexer { get; init; }
    private IParsingService ParsingService { get; init; }
    internal IRuntime Runtime { get; init; }

    public InputHandler(IServiceContainer services, IResourceService resources)
    {
        InputBuffer = resources.GetResource<IInputBuffer>();
        //  get the lexer service
        Lexer = services.GetService<ILexerService>();
        //  get the parsing service
        ParsingService = services.GetService<IParsingService>();
    }

    /// <summary>
    /// Runs the input preprocessor.
    /// </summary>
    public void Process()
    {
        ArraySegment<char> statement = InputBuffer.ReadStatement();

        if (statement[0] == 'x')    // Exit
        {
            Runtime.RequestShutdown();
            return;
        }

        //  Tokenize the input
        var tokens = Lexer.Tokenize<Token>(statement);
        ParsingService.Enqueue(tokens);
    }
}

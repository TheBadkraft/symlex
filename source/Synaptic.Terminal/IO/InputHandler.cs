
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
    private LexerService Lexer { get; init; }
    private IParsingService ParsingService { get; init; }
    internal IRuntime Runtime { get; init; }

    public InputHandler(IInputBuffer inputBuffer, IServiceContainer services)
    {
        InputBuffer = inputBuffer;
        //  get the lexer service
        Lexer = services.GetService<LexerService>();
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
        Data data = new(Lexer.Tokenize<Token>(statement));
        ParsingService.Enqueue(data);
    }
}


using Symlex.Analyzers;

namespace Symlex.IO;

/// <summary>
/// Processes input from the console.
/// </summary>
public class InputPreprocessor
{
    private InputBuffer InputBuffer { get; init; }
    private Lexer Lexer { get; init; }

    internal string Prompt { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputPreprocessor"/> class.
    /// </summary>
    public InputPreprocessor()
    {
        InputBuffer = new InputBuffer();
        Lexer = new Lexer();
    }

    public void Run()
    {
        while (true)
        {
            Console.Write($"{Prompt} "); // Use the prompt only when not in multi-line mode
            ArraySegment<char> statement = InputBuffer.ReadStatement();

            if (statement[0] == 'x')    // Exit
            {
                break;
            }

            //  Tokenize the input
            var tokens = Lexer.Tokenize(statement);
        }
    }
}

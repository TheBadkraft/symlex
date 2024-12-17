
namespace Symlex.IO;

public class InputPreprocessor
{
    private InputBuffer InputBuffer { get; init; }

    internal string Prompt { get; init; }

    public InputPreprocessor()
    {
        InputBuffer = new InputBuffer();
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
        }
    }
}


using Synaptic.Core;
using Synaptic.Analysis.Parsers;

namespace Synaptic.Analysis;

/// <summary>
/// Parses a list of tokens into an instruction object
/// </summary>
public class InputParser : SynapticParser
{
    /// <summary>
    /// The descriptor to be populated.
    /// </summary>
    public ContextDescriptor Descriptor { get; init; } = new();

    public InputParser() { }

    /// <summary>
    /// Parses input tokens into the descriptor.
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public bool ParseInput(IReadOnlyCollection<Token> tokens)
    {
        Descriptor.Data = new(tokens);
        return Parse();
    }
    /// <summary>
    /// Attempts to parse tokens into a Function object.
    /// </summary>
    /// <param name="tokens">The tokens from the lexer, implementing IReadOnlyList<Token>.</param>
    /// <returns>A Function instance if parsing was successful, null otherwise.</returns>
    public override bool Parse()
    {
        int index = 0;
        int start = 0;

        ContextOp op = ContextOp.NoOp;
        ContextTarget target = ContextTarget.None;

        if (IsDefineFunc(ref index, Descriptor.Data))
        {
            op = ContextOp.Define;
            target = ContextTarget.Function;
            start = index;
        }

        //  index to the last token
        index = Descriptor.Data.Count - 1;
        int end;
        //  check the last token is a `]`
        if (!TryConsumeToken(ref index, Descriptor.Data, TokenType.Operator, "]"))
        {
            op = ContextOp.Error;    //  missing closing bracket
            start = 0;
            end = 0;
        }
        else
        {
            end = index - 1;
        }

        //  set context properties
        Descriptor.Action = op;
        Descriptor.Target = target;
        Descriptor.Data = new(TrimSourceArray(start, end, Descriptor.Data));

        return true;
    }

    //  check if the tokens represent a function definition
    private static bool IsDefineFunc(ref int index, Data tokens)
    {
        //  read `[=proc` as first two valid tokens
        return TryConsumeToken(ref index, tokens, TokenType.Operator, "[=") &&
               TryConsumeToken(ref index, tokens, TokenType.Keyword, "proc");
    }

    //  trim underlying source array from array segment tokens
    private static Token[] TrimSourceArray(int index, int end, Data data)
    {
        var subset = data[index..end];
        var startIndex = subset[0].Offset;  //  get the start index of the subset
        var length = subset[^1].Offset + subset[^1].Length - startIndex;    //  calculate the length of the subset
        var source = data[0].Source[startIndex..(startIndex + length)];

        //  adjust tokens in the subset to reflect their new position in the source array
        for (int i = 0; i < subset.Length; i++)
        {
            subset[i] = new Token(new ArraySegment<char>(source, subset[i].Offset - startIndex, subset[i].Length), subset[i].Type);
        }

        return subset;
    }
}

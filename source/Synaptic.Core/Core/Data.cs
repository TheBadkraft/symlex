
using System.Collections;
using Synaptic.Analysis;

namespace Synaptic.Core;

/// <summary>
/// Represents a data object.
/// </summary>
public class Data : IData<Token>
{
    private int index = -1;
    private IReadOnlyCollection<Token> Tokens { get; init; }

    /// <summary>
    /// Gets the token at the specified index
    /// </summary>
    /// <param name="index">The index</param>
    /// <returns>A token</returns>
    public Token this[int index] => Tokens.ElementAt(index);
    /// <summary>
    /// Gets the token at the specified range.
    /// </summary>
    /// <param name="range">The range</param>
    /// <returns>A token array</returns>
    public Token[] this[Range range]
    {
        get
        {
            // Calculate the start and end indices based on the range
            var r = RangeOf(range.Start, range.End, 0);
            // Return an enumerator that yields the elements within the range
            return Enumerable.Range(r.start, r.length).Select(i => this[i]).ToArray();
        }
    }
    /// <summary>
    /// Gets the source of the token collection.
    /// </summary>
    public ArraySegment<char> Source => Tokens.ElementAt(0).Source;
    /// <summary>
    /// Gets the current token.
    /// </summary>
    public Token Current => Tokens.ElementAt(index);
    /// <summary>
    /// Gets the current token object.
    /// </summary>
    object IEnumerator.Current => Current;
    /// <summary>
    /// Gets the token list count.
    /// </summary>
    public int Count => Tokens.Count;

    /// <summary>
    /// Constructs a new data object.
    /// </summary>
    /// <param name="tokena">The token collection.</param>
    public Data(IEnumerable<Token> tokena) : this(tokena.ToList()) { }
    /// <summary>
    /// Constructs a new data object.
    /// </summary>
    /// <param name="tokens">The readonly token collection.</param>
    public Data(IReadOnlyCollection<Token> tokens)
    {
        Tokens = tokens;
    }

    /// <summary>
    /// Determines if the token collection has more tokens.
    /// </summary>
    /// <returns>TRUE if there are more tokens; FALSE otherwise.</returns>
    public bool MoveNext() => ++index < Tokens.Count;
    /// <summary>
    /// Reset the token token enumerator.
    /// </summary>
    public void Reset() => index = 0;
    /// <summary>
    /// Returns a token collection enumerator.
    /// </summary>
    /// <returns>Token enumerator.</returns>
    public IEnumerator<Token> GetEnumerator() => Tokens.GetEnumerator();
    /// <summary>
    /// Returns a collection enumerator.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    /// <summary>
    /// Dispose of the data object.
    /// </summary>
    public void Dispose()
    {
        //  nothing to do here
    }
    /// <summary>
    /// Returns a string representation of the data source.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override string ToString() => new(Source);

    //  Helper method to calculate the start and length based on the range
    private (int start, int length) RangeOf(Index start, Index end, int offset)
    {
        var begin = start.GetOffset(offset);
        var length = end.GetOffset(offset) - begin;

        return (start: begin, length: length);
    }
}

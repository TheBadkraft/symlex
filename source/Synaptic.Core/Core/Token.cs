
using System.Collections;

namespace Synaptic.Analysis;

public struct Token : IEnumerable<char>
{
    private ArraySegment<char> _token;

    /// <summary>
    /// Gets the token value.
    /// </summary>
    public char[] Source
    {
        get => _token.Array;
        set => _token = new ArraySegment<char>(value, Offset, Length);
    }
    /// <summary>
    /// Gets the entire array to which the token belongs.
    /// </summary>
    public IReadOnlyCollection<char> Array => _token.Array;
    /// <summary>
    /// Gets the token span.
    /// </summary>
    public ReadOnlySpan<char> Span => _token;
    /// <summary>
    /// Gets the value of the token at the specified index.
    /// </summary>
    /// <param name="index">The index of the character in the token.</param>
    /// <returns>The character at the specified index.</returns>
    public char this[int index] => _token[index];
    /// <summary>
    /// Gets the token type.
    /// </summary>
    public TokenType Type { get; init; }
    /// <summary>
    /// Gets the length of the token
    /// </summary>
    public int Length => _token.Count;
    /// <summary>
    /// Gets the offset of the token.
    /// </summary>
    public int Offset
    {
        get => _token.Offset;
        set => _token = new ArraySegment<char>(_token.Array, value, Length);
    }
    /// <summary>
    /// Constructs a new token.
    /// </summary>
    /// <param name="token">The token value.</param>
    /// <param name="type">The token type.</param>
    public Token(ArraySegment<char> token, TokenType type)
    {
        _token = token;
        Type = type;
    }

    /// <summary>
    /// Returns the string representation of the token.
    /// </summary>
    /// <returns>The string representation of the token.</returns>
    public override string ToString()
    {
        return $"{Type}: {new string(_token)}";
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IEnumerator<char> GetEnumerator()
    {
        return _token.GetEnumerator();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

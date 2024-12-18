
using System.Collections;

namespace Symlex.Grammar;

public struct Token : IEnumerable<char>
{
    private readonly ArraySegment<char> _token;

    /// <summary>
    /// Gets the entire array to which the token belongs.
    /// </summary>
    internal IReadOnlyCollection<char> Array => _token.Array;
    /// <summary>
    /// Gets the token span.
    /// </summary>
    internal ReadOnlySpan<char> Span => _token;

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
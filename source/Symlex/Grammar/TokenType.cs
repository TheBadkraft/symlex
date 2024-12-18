
namespace Symlex.Grammar;

/// <summary>
/// Represents the type of a token.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Represents an invalid token.
    /// </summary>
    Invalid,
    /// <summary>
    /// Represents an identifier token.
    /// </summary>
    Identifier,
    /// <summary>
    /// Represents a string token.
    /// </summary>
    Operator,
    /// <summary>
    /// Represents a keyword token.
    /// </summary>
    Keyword,
    /// <summary>
    /// Represents a numeric token.
    /// </summary>
    Number,
}

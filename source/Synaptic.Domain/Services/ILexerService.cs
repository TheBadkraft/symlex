
namespace Synaptic.Core;

/// <summary>
/// Represents a service for lexing input.
/// </summary>
public interface ILexerService : ISynapticService
{
    /// <summary>
    /// Tokenizes the input.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="input">The input to tokenize.</param>
    /// <returns>The tokenized input.</returns>
    IReadOnlyList<TResult> Tokenize<TResult>(ArraySegment<char> input);
}

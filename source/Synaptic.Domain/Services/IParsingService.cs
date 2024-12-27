
namespace Synaptic.Core;

/// <summary>
/// Represents a service for parsing context descriptors.
/// </summary>
public interface IParsingService : ISynapticService
{
    void Enqueue<TData>(TData data);
}

public interface ILexerService : ISynapticService
{
    IReadOnlyList<TResult> Tokenize<TResult>(ArraySegment<char> input);
}

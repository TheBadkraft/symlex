
namespace Synaptic.Analysis.Parsers;

/// <summary>
/// Parses a list of tokens a specialized object
/// </summary>
/// <typeparam name="T">The type of object to parse the tokens into</typeparam>
public abstract class Parser : IParser
{
    protected Parser() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public abstract bool Parse();
}


using Synaptic.Core;

namespace Synaptic.Analysis.Parsers;

/// <summary>
/// Represents a parser for context descriptor container.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Parses data into a specialized object
    /// </summary>
    /// <returns>TRUE if data was parsed; FALSE otherwise</returns>
    bool Parse();
}


namespace Synaptic.IO;

/// <summary>
/// Input buffer interface
/// </summary>
public interface IInputBuffer : IDisposable
{
    /// <summary>
    /// Reads a statement from the input buffer.
    /// </summary>
    /// <returns>The statement read from the input buffer.</returns>
    ArraySegment<char> ReadStatement();
}


namespace Synaptic.IO;

/// <summary>
/// A terminal interface.
/// </summary>
public interface ITerminal : IDisposable
{
    /// <summary>
    /// Prompts the user for input.
    /// </summary>
    void Prompt();
    /// <summary>
    /// Prompts the user with a message.
    /// </summary>
    /// <param name="message">The message to prompt the user with.</param>
    void Prompt(string message);
    /// <summary>
    /// Write a message to the terminal.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void Write(string message);
    /// <summary>
    /// Write an error message to the terminal.
    /// </summary>
    /// <param name="message">The error message to write.</param>
    void WriteErr(string message);
    /// <summary>
    /// Write a message to the terminal followed by a newline.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLn(string message);
    /// <summary>
    /// Write an error message to the terminal followed by a newline.
    /// </summary>
    /// <param name="message">The error message to write.</param>
    void WriteErrLn(string message);
}
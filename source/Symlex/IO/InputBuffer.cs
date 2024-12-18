

namespace Symlex.IO;

/// <summary>
/// Input buffer for reading from the console.
/// </summary>
public class InputBuffer : TextReader
{
    /// <summary>
    /// The default buffer size.
    /// </summary>
    internal const long DEFAULT_BUFFER_SIZE = 1024;

    private Stream InputStream { get; init; }
    private bool Echo { get; init; }

    /// <summary>
    /// Gets the length of the input stream.
    /// </summary>
    public long Length => GetBufferLength();
    /// <summary>
    /// Determines whether the input stream supports seeking.
    /// </summary>
    public bool CanSeek => InputStream.CanSeek;

    /// <summary>
    /// Initializes the default instance of the <see cref="InputBuffer"/> class.
    /// </summary>
    /// <remarks>
    /// Echo is set to FALSE by default to prevent echoing the input characters to the console.
    /// </remarks>
    /// <param name="echo">TRUE to echo the input characters to the console; otherwise, FALSE.</param>
    public InputBuffer(bool echo = false) : this(Console.OpenStandardInput(), echo) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InputBuffer"/> class with
    /// the specified input stream.
    /// </summary>
    /// <remarks>
    /// Echo is set to TRUE by default to echo the input characters to the console output stream.
    /// </remarks>
    /// <param name="inputStream">The input stream to read from.</param>
    /// <param name="echo">TRUE to echo the input characters to the console; otherwise, FALSE.</param>
    public InputBuffer(Stream inputStream, bool echo = true)
    {
        Echo = echo;
        InputStream = inputStream;
    }

    /// <summary>
    /// Reads the next character from the input stream.
    /// </summary>
    /// <returns>The next character from the input stream, or -1 if no more characters are available.</returns>
    override public int Read()
    {
        var buffer = new byte[1];
        var scan = InputStream.Read(buffer, 0, 1) == 1 ? buffer[0] : -1;
        if (scan != -1)
        {
            // Grok: add backspace handling here
            if (scan == (byte)'\b' && Console.CursorLeft > 0) // Handle backspace
            {
                Console.Write("\b");    // Move cursor back
                Console.Write(" ");     // Overwrite with space
                Console.Write("\b");    // Move cursor back again
            }
            else if (Echo)
            {
                Console.Write((char)scan); // Echo the character
            }
        }
        return scan;
    }
    /// <summary>
    /// Reads the next character from the input stream without changing the position of the stream.
    /// </summary>
    /// <returns>An integer representing the next character to be read, or -1 if no more characters are available.</returns>
    public override int Peek()
    {
        if (InputStream.CanSeek)
        {
            long currentPosition = InputStream.Position;
            int result = Read();
            InputStream.Seek(currentPosition, SeekOrigin.Begin);
            return result != -1 ? result : -1; // Return -1 if EOF, otherwise the character
        }

        return -1; // If stream is not seekable, return -1 as we can't peek
    }

    private long GetBufferLength()
    {
        if (InputStream.CanSeek)
        {
            long currentPosition = InputStream.Position;
            long length = InputStream.Length;
            InputStream.Seek(currentPosition, SeekOrigin.Begin);
            return length;
        }

        return -1; // If stream is not seekable, return -1 as we can't peek
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="InputBuffer"/> and
    /// </summary>
    /// <param name="disposing">TRUE to release both managed and unmanaged resources; FALSE to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        //  add managed/unmanaged resource cleanup here
        InputStream.Dispose();
        base.Dispose(disposing);
    }
}

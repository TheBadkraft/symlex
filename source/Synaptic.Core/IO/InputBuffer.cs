

namespace Synaptic.IO;

/// <summary>
/// Input buffer for reading from the console.
/// </summary>
public class InputBuffer : IInputBuffer
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
        InputStream = inputStream;
        Echo = echo;
    }

    /// <summary>
    /// Reads and formats the current statement from the input buffer.
    /// </summary>
    /// <param name="inputBuffer">The input buffer to read from.</param>
    /// <returns>An array segment containing the current statement.</returns>
    public unsafe ArraySegment<char> ReadStatement()
    {
        /*
            I made this method unsafe with a pinned buffer because it was easy to do. We 
            can always refactor this to a safe method later. The performance gain is minimal.
            The main reason for doing this is to show that we can do it.
        */
        bool inMultiLineMode = false;
        int bracketBalance = 0;
        long length = CanSeek ? Length : InputBuffer.DEFAULT_BUFFER_SIZE;
        char[] buffer = new char[Math.Min(length, int.MaxValue)];
        int c = -1, ndx = 0;

        fixed (char* p = buffer)
        {
            char* current = p;
            char* end = p + buffer.Length;

            while (true)
            {
                c = Read();
                if (c == -1) break; // EOF

                // Check if we need to expand the buffer
                if (current >= end)
                {
                    /* 
                        Technically, we should never get here ... 
                        and if we do, we should throw an exception ...
                        because we cannot reassign a pinned buffer!
                     */

                    /*                      
                        //  get the current buffer handle and free it
                        GCHandle handle = GCHandle.FromIntPtr((IntPtr)p);
                        handle.Free();

                        //  resize the buffer (*2)
                        char[] newBuffer = new char[buffer.Length * 2];

                        //  copy elements from old buffer to new buffer
                        Array.Copy(buffer, 0, newBuffer, 0, buffer.Length);

                        //  re-pin the new buffer
                        buffer = newBuffer;
                        handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        p = (char*)handle.AddrOfPinnedObject();
                     */
                }

                if (c == '\n' || c == '\r') // Check for new line
                {
                    if (ndx > 0)
                    {
                        if (!inMultiLineMode && bracketBalance == 0)
                        {
                            return new ArraySegment<char>(buffer, 0, ndx);
                        }
                    }
                }
                else
                {
                    *current++ = (char)c;
                    ndx = (int)(current - p);

                    if (c == '[')
                    {
                        inMultiLineMode = true;
                        bracketBalance++;
                    }
                    else if (c == ']')
                    {
                        bracketBalance--;
                        if (bracketBalance == 0)
                        {
                            inMultiLineMode = false;
                            return new ArraySegment<char>(buffer, 0, ndx);
                        }
                    }
                }
            }
        }
        //  if we reach EOF, return what we have
        return new ArraySegment<char>(buffer, 0, ndx);
    }

    /// <summary>
    /// [INTERNAL] Reads the next character from the input stream.
    /// </summary>
    /// <returns>The next character from the input stream, or -1 if no more characters are available.</returns>
    internal int Read()
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
    /// [INTERNAL] Reads the next character from the input stream without changing the position of the stream.
    /// </summary>
    /// <returns>An integer representing the next character to be read, or -1 if no more characters are available.</returns>
    internal int Peek()
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
    public void Dispose()
    {
        //  add managed/unmanaged resource cleanup here
        InputStream.Dispose();
    }
}

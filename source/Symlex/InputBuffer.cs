using System;

namespace Symlex.IO;

public class InputBuffer : TextReader
{
    // private readonly byte[] _buffer = new byte[1024]; // adjust buffer size as needed
    // private int _bufferPosition;
    // private int _bufferLength;
    // private bool _inMultiLineMode = false;
    // private int _bracketBalance = 0;

    private Stream InputStream { get; init; }

    /// <summary>
    /// Initializes the default instance of the <see cref="InputBuffer"/> class.
    /// </summary>
    public InputBuffer() : this(Console.OpenStandardInput()) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InputBuffer"/> class with
    /// the specified input stream.
    /// </summary>
    /// <param name="inputStream">The input stream to read from.</param>
    public InputBuffer(Stream inputStream)
    {
        InputStream = inputStream;
    }

    public long Length => InputStream.Length;


    /*     public ArraySegment<char> ReadStatement()
        {
            var statement = new List<char>();
            while (true)
            {
                if (_bufferPosition >= _bufferLength)
                {
                    _bufferLength = _inputStream.Read(_buffer, 0, _buffer.Length);
                    _bufferPosition = 0;
                    if (_bufferLength == 0) break; // EOF
                }

                char c = (char)_buffer[_bufferPosition++];
                if (c == '\n' || c == '\r') // Check for new line
                {
                    if (statement.Count > 0)
                    {
                        string line = new string(statement.ToArray());
                        if (line.StartsWith("["))
                        {
                            _inMultiLineMode = true;
                            _bracketBalance++;
                        }

                        _bracketBalance += line.Count(ch => ch == '[');
                        _bracketBalance -= line.Count(ch => ch == ']');

                        if (_inMultiLineMode && _bracketBalance == 0)
                        {
                            _inMultiLineMode = false;
                            return new ArraySegment<char>(statement.ToArray());
                        }
                        else if (!_inMultiLineMode)
                        {
                            return new ArraySegment<char>(statement.ToArray());
                        }
                        else
                        {
                            statement.Add('\n'); // Add newline for multi-line statements
                        }
                    }
                }
                else if (c == '\b' && statement.Count > 0) // Backspace
                {
                    statement.RemoveAt(statement.Count - 1);
                    Console.Write("\b \b"); // Erase the last character from console
                }
                else
                {
                    statement.Add(c);
                    Console.Write(c); // Echo the character
                }
            }
            return new ArraySegment<char>(statement.ToArray()); // Return what we have if EOF
        }
     */

    /// <summary>
    /// Reads the next character from the input stream.
    /// </summary>
    /// <returns></returns>
    override public int Read()
    {
        var buffer = new byte[1];
        var scan = InputStream.Read(buffer, 0, 1) == 1 ? buffer[0] : -1;
        if (scan != -1)
        {
            Console.Write((char)scan); // Echo the character
        }
        return scan;
    }

    /* 
        public override int Peek()
        {
            if (_bufferPosition >= _bufferLength)
            {
                _bufferLength = inputStream.Read(_buffer, 0, _buffer.Length);
                _bufferPosition = 0;
            }
            return _bufferPosition < _bufferLength ? _buffer[_bufferPosition] : -1;
        }

     */

    protected override void Dispose(bool disposing)
    {
        InputStream.Dispose();
        base.Dispose(disposing);
    }
}

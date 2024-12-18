
namespace Symlex.IO;

public static class IOExtensions
{
    /// <summary>
    /// Reads and formats the current statement from the input buffer.
    /// </summary>
    /// <param name="inputBuffer">The input buffer to read from.</param>
    /// <returns>An array segment containing the current statement.</returns>
    public unsafe static ArraySegment<char> ReadStatement(this InputBuffer inputBuffer)
    {
        /*
            I made this method unsafe with a pinned buffer because it was easy to do. We 
            can always refactor this to a safe method later. The performance gain is minimal.
            The main reason for doing this is to show that we can do it.
        */
        bool inMultiLineMode = false;
        int bracketBalance = 0;
        long length = inputBuffer.CanSeek ? inputBuffer.Length : InputBuffer.DEFAULT_BUFFER_SIZE;
        char[] buffer = new char[Math.Min(length, int.MaxValue)];
        int c = -1, ndx = 0;

        fixed (char* p = buffer)
        {
            char* current = p;
            char* end = p + buffer.Length;

            while (true)
            {
                c = inputBuffer.Read();
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
}

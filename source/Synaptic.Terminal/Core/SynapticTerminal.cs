// Last update: 2024-12-16 14:30

using Synaptic.IO;

namespace Synaptic.Core;

public partial class Runtime
{
    private class SynapticTerminal : ITerminal
    {
        private static Encoding Encoder { get; } = Encoding.UTF8;

        //  standard output stream
        private Stream StdOut { get; } = Console.OpenStandardOutput();
        //  error output stream
        private Stream ErrOut { get; } = Console.OpenStandardError();

        /// <summary>
        /// Initializes a new instance of the <see cref="SynapticTerminal"/> class.
        /// </summary>
        internal SynapticTerminal()
        {
            //  set the console output encoding
            Console.OutputEncoding = Encoder;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Prompt()
        {
            Write($"{PROMPT}");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public void Prompt(string message)
        {
            Write($"{message}\n{PROMPT}");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public void Write(string message)
        {
            //  write to standard output stream
            message = $"{message}";
            StdOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public void WriteErr(string message)
        {
            //  write to error output stream
            message = $"\n*** ERR: {message}";
            ErrOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public void WriteLn(string message)
        {
            //  write line to standard output stream
            message = $"{message}\n";
            StdOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public void WriteErrLn(string message)
        {
            //  write line to error output stream
            message = $"\n*** ERR: {message}\n";
            ErrOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            //  TODO: implement ... ???
        }

        //  get the message byte array
        private static byte[] GetBytes(string message)
        {
            return Encoder.GetBytes(message);
        }
    }
}

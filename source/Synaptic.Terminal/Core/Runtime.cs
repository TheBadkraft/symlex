// Last update: 2024-12-16 14:30

using Synaptic.IO;

namespace Synaptic.Core;

/// <summary>
/// The main runtime of the Synaptic interpreter.
/// </summary>
public class Runtime : IRuntime
{
    const string PROMPT = "E: ";

    private bool IsShutdownRequested { get; set; } = false;
    private InputHandler InputHandler { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public ITerminal Terminal { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Runtime"/> class.
    /// </summary>
    /// <param name="dataFilePath">The path to the data file.</param>
    public Runtime()
    {
        //  initialize the terminal first
        Terminal = new SynapticTerminal();
        //  on runtime initialization, register self with SynapticHub as the Runtime
        SynapticHub.Instance.RegisterRuntime(this);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Initialize(IServiceContainer services, IResourceService resources)
    {
        var InputBuffer = resources.RegisterResource<IInputBuffer>(new InputBuffer());
        InputHandler = new(InputBuffer, services) { Runtime = this };
    }
    /// <summary>
    /// Launches the Synaptic interpreter runtime.
    /// </summary>
    public void Launch()
    {
        bool isRunning = !IsShutdownRequested;

        while (isRunning)
        {
            InputHandler.Process();
            if (isRunning = !IsShutdownRequested) Terminal.Prompt();
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void RequestShutdown()
    {
        //  set shutdown flag TRUE
        IsShutdownRequested = true;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        Terminal.Dispose();
    }

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
        public void Prompt(string message)
        {
            Write($"{message}\n{PROMPT}");
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Write(string message)
        {
            //  write to standard output stream
            message = $"{message}";
            StdOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void WriteErr(string message)
        {
            //  write to error output stream
            message = $"\n*** ERR: {message}";
            ErrOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void WriteLn(string message)
        {
            //  write line to standard output stream
            message = $"{message}\n";
            StdOut.Write(GetBytes(message));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
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

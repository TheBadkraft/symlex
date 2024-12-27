using Synaptic.Core;

namespace Synaptic.Analysis;

/// <summary>
/// Represents a base descriptor.
/// </summary>
public class ContextDescriptor
{
    private List<CompilerOp> instructions { get; } = new();

    /// <summary>
    /// The action to be performed on the target.
    /// </summary>
    public CompilerOp Action { get; set; }
    /// <summary>
    /// The descriptor target type.
    /// </summary>
    public CompilerTarget Target { get; set; }
    /// <summary>
    /// The data to the descriptor.
    /// </summary>
    public Data Data { get; set; }

    public ContextDescriptor() : this(CompilerOp.NoOp, CompilerTarget.None) { }
    public ContextDescriptor(CompilerOp action, CompilerTarget target)
    {
        Action = action;
        Target = target;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ContextDescriptor"/> class.
    /// </summary>
    /// <param name="create">The descriptor creation function.</param>
    /// <returns>A new instance of the <see cref="ContextDescriptor"/> class.</returns>
    public static ContextDescriptor Create(Func<ContextDescriptor> create)
    {
        var descriptor = create();

        if (descriptor is null)
        {
            descriptor.Action = CompilerOp.Error;
            return descriptor;
        }

        return descriptor;
    }

    /// <summary>
    /// Adds a compiler operation to the descriptor.
    /// </summary>
    /// <param name="op">The compiler operation.</param>
    public void AddOp(CompilerOp op) => instructions.Add(op);
    /// <summary>
    /// Get the operation stack.
    /// </summary>
    /// <returns>A CompilerOp stack.</returns>
    public Stack<CompilerOp> GetOpStack() => new(instructions);

}

using Synaptic.Core;

namespace Synaptic.Analysis;

/// <summary>
/// Represents a base descriptor.
/// </summary>
public class ContextDescriptor
{
    private List<ContextOp> instructions { get; } = new();

    /// <summary>
    /// The action to be performed on the target.
    /// </summary>
    public ContextOp Action { get; set; }
    /// <summary>
    /// The descriptor target type.
    /// </summary>
    public ContextTarget Target { get; set; }
    /// <summary>
    /// The data to the descriptor.
    /// </summary>
    public Data Data { get; set; }

    public ContextDescriptor() : this(ContextOp.NoOp, ContextTarget.None) { }
    public ContextDescriptor(ContextOp action, ContextTarget target)
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
            descriptor.Action = ContextOp.Error;
            return descriptor;
        }

        return descriptor;
    }

    /// <summary>
    /// Adds a compiler operation to the descriptor.
    /// </summary>
    /// <param name="op">The compiler operation.</param>
    public void AddOp(ContextOp op) => instructions.Add(op);
    /// <summary>
    /// Get the operation stack.
    /// </summary>
    /// <returns>A CompilerOp stack.</returns>
    public Stack<ContextOp> GetOpStack() => new(instructions);
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override string ToString() => $"({Target}) => {Action}";
}

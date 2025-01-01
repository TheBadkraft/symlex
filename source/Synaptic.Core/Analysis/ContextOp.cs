
namespace Synaptic.Analysis;

/// <summary>
/// The <see cref="ContextOp"/> is a byte code instruction used 
/// by the parsing service to indicate the operation(s) to be performed
/// either in subsequent parsers or in the compiler.
/// </summary>
/// <remarks>
/// We can add more operations as needed. If a <see cref="ContextOp"/> 
/// is needed on the fly, we will have to determine how to handle it.
/// </remarks>
public struct ContextOp
{
    #region helper members
    private const string NOOP = "NoOp";
    private const string BEGDEF = "Create";
    private const string ENDDEF = "Build";
    private const string ERROR = "Error";

    private enum OpCodes
    {
        NoOp = 0x00,
        Define = 0x01,
        End = 0x91,
        Error = 0xFE
    }
    #endregion helper members

    private OpCodes OpCode { get; init; }

    /// <summary>
    /// Gets the name of the operation.
    /// </summary>
    public string Name { get; init; }

    #region op members
    /// <summary>
    /// Represents the NoOp operation.
    /// </summary>
    public static ContextOp NoOp { get; } = new(NOOP, OpCodes.NoOp);
    /// <summary>
    /// Represents the Create operation.
    /// </summary>
    public static ContextOp Define { get; } = new(BEGDEF, OpCodes.Define);
    /// <summary>
    /// Represents the Build operation.
    /// </summary>
    public static ContextOp End { get; } = new(ENDDEF, OpCodes.End);
    /// <summary>
    /// Represents an Error condition.
    /// </summary>
    public static ContextOp Error { get; } = new(ERROR, OpCodes.Error);
    #endregion op members

    //  private to restrict instantiation; only the static instances are allowed
    private ContextOp(string name, OpCodes opCode)
    {
        OpCode = opCode;
        Name = name;
    }

    /// <summary>
    /// Converts the <see cref="ContextOp"/> to a byte.
    /// </summary>
    /// <param name="comOp">The <see cref="ContextOp"/> to convert.</param>
    public static implicit operator byte(ContextOp comOp) => (byte)comOp.OpCode;
    /// <summary>
    /// Converts a byte to a <see cref="ContextOp"/>.
    /// </summary>
    /// <param name="byteCode">The byte to convert.</param>
    public static implicit operator ContextOp(byte byteCode) => (OpCodes)byteCode switch
    {
        OpCodes.NoOp => NoOp,
        OpCodes.Define => Define,
        OpCodes.End => End,
        OpCodes.Error => Error,
        _ => NoOp
    };
    /// <summary>
    /// Determines if the <see cref="ContextOp"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>TRUE if the objects are equal; FALSE otherwise.</returns>
    public override bool Equals(object obj)
    {
        return obj is ContextOp comOp && comOp.OpCode == OpCode;
    }
    /// <summary>
    /// Gets the hash code of the <see cref="ContextOp"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => OpCode.GetHashCode();
    /// <summary>
    /// Converts the <see cref="ContextOp"/> to a string.
    /// </summary>
    /// <returns>The string representation of the <see cref="ContextOp"/>.</returns>
    public override string ToString() => $"Context.{Name} (0x{(byte)OpCode:x2})";
}


namespace Synaptic.Analysis;

/// <summary>
/// Represents a compiler operation. These are determined during 
/// parsing and analysis.
/// </summary>
public struct CompilerOp
{
    #region helper members
    private const string NOOP = "NoOp";
    private const string PUSH = "Push";
    private const string POP = "Pop";
    private const string DECLARE = "Declare";
    private const string CALL = "Call";
    private const string ASSIGN = "Assign";
    private const string RETURN = "Return";
    private const string JUMP = "Jump";
    private const string ERROR = "Error";


    private enum OpCodes
    {
        NoOp = 0x00,
        Push = 0x02,
        Pop = 0x03,
        Declare = 0x04,
        Call = 0x05,
        Assign = 0x06,
        Return = 0x07,
        Jump = 0x08,
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
    public static CompilerOp NoOp { get; } = new(NOOP, OpCodes.NoOp);
    /// <summary>
    /// Represents the Push operation.
    /// </summary>
    public static CompilerOp Push { get; } = new(PUSH, OpCodes.Push);
    /// <summary>
    /// Represents the Pop operation.
    /// </summary>
    public static CompilerOp Pop { get; } = new(POP, OpCodes.Pop);
    /// <summary>
    /// Represents the Declare operation.
    /// </summary>
    public static CompilerOp Desclare { get; } = new(DECLARE, OpCodes.Declare);
    /// <summary>
    /// Represents the Call operation.
    /// </summary>
    public static CompilerOp Call { get; } = new(CALL, OpCodes.Call);
    /// <summary>
    /// Represents the Assign operation.
    /// </summary>
    public static CompilerOp Assign { get; } = new(ASSIGN, OpCodes.Assign);
    /// <summary>
    /// Represents the Return operation.
    /// </summary>
    public static CompilerOp Return { get; } = new(RETURN, OpCodes.Return);
    /// <summary>
    /// Represents the Jump operation.
    /// </summary>
    public static CompilerOp Jump { get; } = new(JUMP, OpCodes.Jump);
    /// <summary>
    /// Represents an Error condition.
    /// </summary>
    public static CompilerOp Error { get; } = new(ERROR, OpCodes.Error);
    #endregion op members

    //  private to restrict instantiation; only the static instances are allowed
    private CompilerOp(string name, OpCodes opCode)
    {
        OpCode = opCode;
        Name = name;
    }

    /// <summary>
    /// Converts the <see cref="CompilerOp"/> to a byte.
    /// </summary>
    /// <param name="comOp">The <see cref="CompilerOp"/> to convert.</param>
    public static implicit operator byte(CompilerOp comOp) => (byte)comOp.OpCode;
    /// <summary>
    /// Converts a byte to a <see cref="CompilerOp"/>.
    /// </summary>
    /// <param name="byteCode">The byte to convert.</param>
    public static implicit operator CompilerOp(byte byteCode) => (OpCodes)byteCode switch
    {
        OpCodes.NoOp => NoOp,
        OpCodes.Push => Push,
        OpCodes.Pop => Pop,
        OpCodes.Declare => Desclare,
        OpCodes.Call => Call,
        OpCodes.Assign => Assign,
        OpCodes.Return => Return,
        OpCodes.Jump => Jump,
        OpCodes.Error => Error,
        _ => NoOp
    };
    /// <summary>
    /// Determines if the <see cref="CompilerOp"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>TRUE if the objects are equal; FALSE otherwise.</returns>
    public override bool Equals(object obj)
    {
        return obj is CompilerOp comOp && comOp.OpCode == OpCode;
    }
    /// <summary>
    /// Gets the hash code of the <see cref="ContextOp"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => OpCode.GetHashCode();
    /// <summary>
    /// Converts the <see cref="CompilerOp"/> to a string.
    /// </summary>
    /// <returns>The string representation of the <see cref="CompilerOp"/>.</returns>
    public override string ToString() => $"Compiler.{Name} (0x{(byte)OpCode:x2})";
}

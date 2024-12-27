
using System.Diagnostics.Tracing;

namespace Synaptic.Analysis;

/// <summary>
/// The <see cref="CompilerOp"/> is a byte code instruction used 
/// by the Synaptic compiler.
/// </summary>
/// <remarks>
/// We can add more operations as needed. I a <see cref="CompilerOp"/> 
/// is needed on the fly, instantiate via reflection as the constructor
/// is private.
/// </remarks>
public struct CompilerOp
{
    #region helper properties
    private const string NOOP = "NoOp";
    private const string CREATE = "Create";
    private const string PUSH = "Push";
    private const string POP = "Pop";
    private const string CALL = "Call";
    private const string ASSIGN = "Assign";
    private const string RETURN = "Return";
    private const string JUMP = "Jump";
    private const string BUILD = "Build";
    private const string ERROR = "Error";

    private const byte NoOpCode = 0x00;
    private const byte CreateOpCode = 0x01;
    private const byte PushOpCode = 0x02;
    private const byte PopOpCode = 0x03;
    private const byte CallOpCode = 0x04;
    private const byte AssignOpCode = 0x05;
    private const byte ReturnOpCode = 0x06;
    private const byte JumpOpCode = 0x07;
    private const byte BuildOpCode = 0x99;
    private const byte ErrorOpCode = 0xFE;
    #endregion helper properties

    private byte OpCode { get; init; }

    /// <summary>
    /// Gets the name of the operation.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Represents the NoOp operation.
    /// </summary>
    public static CompilerOp NoOp { get; } = new(NOOP, NoOpCode);
    /// <summary>
    /// Represents the Create operation.
    /// </summary>
    public static CompilerOp Create { get; } = new(CREATE, CreateOpCode);
    /// <summary>
    /// Represents the Push operation.
    /// </summary>
    public static CompilerOp Push { get; } = new(PUSH, PushOpCode);
    /// <summary>
    /// Represents the Pop operation.
    /// </summary>
    public static CompilerOp Pop { get; } = new(POP, PopOpCode);
    /// <summary>
    /// Represents the Call operation.
    /// </summary>
    public static CompilerOp Call { get; } = new(CALL, CallOpCode);
    /// <summary>
    /// Represents the Assign operation.
    /// </summary>
    public static CompilerOp Assign { get; } = new(ASSIGN, AssignOpCode);
    /// <summary>
    /// Represents the Return operation.
    /// </summary>
    public static CompilerOp Return { get; } = new(RETURN, ReturnOpCode);
    /// <summary>
    /// Represents the Jump operation.
    /// </summary>
    public static CompilerOp Jump { get; } = new(JUMP, JumpOpCode);
    /// <summary>
    /// Represents the Build operation.
    /// </summary>
    public static CompilerOp Build { get; } = new(BUILD, 0x10);
    /// <summary>
    /// Represents the Error operation.
    /// </summary>
    public static CompilerOp Error { get; } = new(ERROR, 0x20);

    //  private to restrict instantiation; only the static instances are allowed
    private CompilerOp(string name, byte opCode)
    {
        OpCode = opCode;
        Name = name;
    }

    /// <summary>
    /// Converts the <see cref="CompilerOp"/> to a byte.
    /// </summary>
    /// <param name="comOp">The <see cref="CompilerOp"/> to convert.</param>
    public static implicit operator byte(CompilerOp comOp) => comOp.OpCode;
    /// <summary>
    /// Converts a byte to a <see cref="CompilerOp"/>.
    /// </summary>
    /// <param name="byteCode">The byte to convert.</param>
    public static implicit operator CompilerOp(byte byteCode) => byteCode switch
    {
        NoOpCode => NoOp,
        CreateOpCode => Create,
        PushOpCode => Push,
        PopOpCode => Pop,
        CallOpCode => Call,
        AssignOpCode => Assign,
        ReturnOpCode => Return,
        JumpOpCode => Jump,
        BuildOpCode => Build,
        ErrorOpCode => Error,
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
    /// Gets the hash code of the <see cref="CompilerOp"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => OpCode.GetHashCode();
    /// <summary>
    /// Converts the <see cref="CompilerOp"/> to a string.
    /// </summary>
    /// <returns>The string representation of the <see cref="CompilerOp"/>.</returns>
    public override string ToString() => $"{Name} (0x{OpCode:x2})";
}

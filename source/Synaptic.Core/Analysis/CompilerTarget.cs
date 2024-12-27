
namespace Synaptic.Analysis;

public struct CompilerTarget
{
    #region helper properties
    private const string NONE = "None";
    private const string FUNCTION = "Function";
    private const string VARIABLE = "Variable";
    private const string CONSTANT = "Constant";
    private const string TYPE = "Type";
    private const byte NoneCode = 0x00;
    private const byte FunctionCode = 0x01;
    private const byte VariableCode = 0x02;
    private const byte ConstantCode = 0x03;
    private const byte TypeCode = 0x04;
    #endregion helper properties

    private byte TargetCode { get; init; }

    /// <summary>
    /// Gets the name of the target.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Represents the None target.
    /// </summary>
    public static CompilerTarget None { get; } = new() { Name = NONE, TargetCode = NoneCode };
    /// <summary>
    /// Represents the Function target.
    /// </summary>
    public static CompilerTarget Function { get; } = new() { Name = FUNCTION, TargetCode = FunctionCode };
    /// <summary>
    /// Represents the Variable target.
    /// </summary>
    public static CompilerTarget Variable { get; } = new() { Name = VARIABLE, TargetCode = VariableCode };
    /// <summary>
    /// Represents the Constant target.
    /// </summary>
    public static CompilerTarget Constant { get; } = new() { Name = CONSTANT, TargetCode = ConstantCode };
    /// <summary>
    /// Represents the Type target.
    /// </summary>
    public static CompilerTarget Type { get; } = new() { Name = TYPE, TargetCode = TypeCode };

    //  private to restrict instantiation; only the static instances are allowed
    private CompilerTarget(string name, byte targetCode)
    {
        TargetCode = targetCode;
        Name = name;
    }

    /// <summary>
    /// Converts the <see cref="CompilerTarget"/> to a byte.
    /// </summary>
    /// <param name="comTarg">The <see cref="CompilerTarget"/> to convert.</param>
    public static implicit operator byte(CompilerTarget comTarg) => comTarg.TargetCode;
    /// <summary>
    /// Converts a byte to a <see cref="CompilerTarget"/>.
    /// </summary>
    /// <param name="targetCode">The byte to convert.</param>
    public static implicit operator CompilerTarget(byte targetCode) => targetCode switch
    {
        NoneCode => None,
        FunctionCode => Function,
        VariableCode => Variable,
        ConstantCode => Constant,
        TypeCode => Type,
        _ => None
    };
    /// <summary>
    /// Determines if the <see cref="CompilerTarget"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>TRUE if the objects are equal; FALSE otherwise.</returns>
    public override bool Equals(object obj)
    {
        return obj is CompilerTarget comTarg && comTarg.TargetCode == TargetCode;
    }
    /// <summary>
    /// Gets the hash code of the <see cref="CompilerTarget"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => TargetCode.GetHashCode();
    /// <summary>
    /// Converts the <see cref="CompilerTarget"/> to a string.
    /// </summary>
    /// <returns>The string representation of the <see cref="CompilerTarget"/>.</returns>
    public override string ToString() => $"{Name} (0x{TargetCode:x2})";
}
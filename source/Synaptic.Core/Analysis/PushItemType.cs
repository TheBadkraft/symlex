
namespace Synaptic.Analysis;

/// <summary>
/// Represents a push item type.
/// </summary>
public struct PushItemType
{
    private byte TypeCode { get; init; }

    /// <summary>
    /// Gets the name of the item type.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Represents an Identifier type.
    /// </summary>
    public static readonly PushItemType Identifier = new PushItemType("Identifier", 0x00);
    /// <summary>
    /// Represents a Parameter type.
    /// </summary>
    public static readonly PushItemType Parameter = new PushItemType("Parameter", 0x01);
    /// <summary>
    /// Represents a BodyToken type.
    /// </summary>
    public static readonly PushItemType BodyToken = new PushItemType("BodyToken", 0x02);
    /// <summary>
    /// Represents a Pattern type.
    /// </summary>
    public static readonly PushItemType Pattern = new PushItemType("Pattern", 0x03);
    /// <summary>
    /// Represents a Symbol type.
    /// </summary>
    public static readonly PushItemType Symbol = new PushItemType("Symbol", 0x04);

    //  private to restrict instantiation; only the static instances are allowed
    private PushItemType(string name, byte typeCode)
    {
        TypeCode = typeCode;
        Name = name;
    }

    /// <summary>
    /// Converts the <see cref="PushItemType"/> to a byte.
    /// </summary>
    /// <param name="itemType">The <see cref="PushItemType"/> to convert.</param>
    public static implicit operator byte(PushItemType itemType) => itemType.TypeCode;
    /// <summary>
    /// Determines if the <see cref="PushItemType"/> is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>TRUE if the objects are equal; FALSE otherwise.</returns>
    public override bool Equals(object obj)
    {
        return obj is PushItemType type && TypeCode == type.TypeCode;
    }
    /// <summary>
    /// Gets the hash code of the <see cref="PushItemType"/>.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => TypeCode.GetHashCode();
    /// <summary>
    /// Converts the <see cref="PushItemType"/> to a string.
    /// </summary>
    /// <returns>The string representation of the <see cref="PushItemType"/>.</returns>
    public override string ToString() => $"{Name} (0x{TypeCode:X2})";
}
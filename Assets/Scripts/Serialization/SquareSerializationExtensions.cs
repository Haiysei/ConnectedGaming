using Unity.Netcode;
using UnityChess;

public static class SquareSerializationExtensions
{
    public static void WriteValueSafe(this FastBufferWriter writer, in Square square)
    {
        writer.WriteValueSafe(square.File); // Serialize the file (column)
        writer.WriteValueSafe(square.Rank); // Serialize the rank (row)
    }

    public static void ReadValueSafe(this FastBufferReader reader, out Square square)
    {
        reader.ReadValueSafe(out int file); // Deserialize the file (column)
        reader.ReadValueSafe(out int rank); // Deserialize the rank (row)
        square = new Square(file, rank);   // Reconstruct the Square object
    }
}
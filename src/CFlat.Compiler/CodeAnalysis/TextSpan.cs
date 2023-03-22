namespace CFlat.Compiler.CodeAnalysis;

public struct TextSpan
{
    public TextSpan(Int32 start, Int32 length)
    {
        Start = start;
        Length = length;
    }

    public Int32 Start { get; }
    public Int32 Length { get; }
    public Int32 End => Start + Length;
}

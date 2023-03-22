namespace CFlat.Compiler.CodeAnalysis;

public sealed class Diagnostic
{
    public Diagnostic(TextSpan span, String message)
    {
        Span = span;
        Message = message;
    }

    public TextSpan Span { get; }
    public String Message { get; }

    public override string ToString() => Message;
    
}

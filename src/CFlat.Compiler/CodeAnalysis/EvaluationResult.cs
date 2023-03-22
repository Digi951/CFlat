namespace CFlat.Compiler.CodeAnalysis;

public sealed class EvaluationResult
{
    public EvaluationResult(IEnumerable<Diagnostic> diagnostics, Object value)
    {
        Diagnostics = diagnostics;
        Value = value;
    }

    public IEnumerable<Diagnostic> Diagnostics { get; }
    public object Value { get; }
}


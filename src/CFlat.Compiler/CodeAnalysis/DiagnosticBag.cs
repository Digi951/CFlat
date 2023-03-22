using System.Collections;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public void AddRange(DiagnosticBag diagnostics)
    {
        _diagnostics.AddRange(diagnostics._diagnostics);
    }

    private void Report(TextSpan span, String message)
    {
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }

    public void ReportInvalidNumber(TextSpan span, String text, Type type)
    {
        var message = $"The number {text} isn't a valid {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(Int32 position, Char character)
    {
        var span = new TextSpan(position, 1);
        var message = $"ERROR: Bad character input: {character}.";
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"ERROR: Unexpexted token <{actualKind}>, expected <{expectedKind}>.";
        Report(span, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string? operatorText, Type operatorType)
    {
        var message = $"Unary operator '{operatorText}' is not defined for type {operatorType}";
        Report(span, message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string? operatorText, Type leftType, Type rightType)
    {
        var message = $"Binary operator '{operatorText}' is not defined for types {leftType} and {rightType}";
        Report(span, message);
    }

    public void ReportUndefinedName(TextSpan span, string name)
    {
        var message = $"Variable '{name}' doesn't exist.";
        Report(span, message);
    }
}
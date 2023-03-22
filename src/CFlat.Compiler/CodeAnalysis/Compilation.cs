using CFlat.Compiler.CodeAnalysis.Binding;
using CFlat.Compiler.CodeAnalysis.Syntax;

namespace CFlat.Compiler.CodeAnalysis;

public class Compilation
{
    public Compilation(SyntaxTree syntax)
    {
        Syntax = syntax;
    }

    public SyntaxTree Syntax { get; }

    public EvaluationResult Evaluate()
    {
        Binder binder = new();
        BoundExpression boundExpression = binder.BindExpression(Syntax.Root);

        var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
        if (diagnostics.Any())
        {
            return new EvaluationResult(diagnostics, null);
        }

        Evaluator evaluator = new(boundExpression);
        var value = evaluator.Evaluate();
        return new EvaluationResult(Array.Empty<Diagnostic>(), value);
    }
}

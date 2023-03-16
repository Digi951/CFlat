using System;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

public class UnaryExpressionSyntax : ExpressionSyntax
{
    public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
    {
        OperatorToken = operatorToken;
        Operand = operand;
    }

    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Operand { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OperatorToken;
        yield return Operand;
    }
}


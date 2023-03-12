using System;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

public sealed class Evaluator
{
    private readonly ExpressionSyntax _root;

    public Evaluator(ExpressionSyntax root)
    {
        _root = root;
    }

    public Int32 Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private Int32 EvaluateExpression(ExpressionSyntax node)
    {
        if (node is LiteralExpressionSyntax n)
        {
            return (Int32)n.LiteralToken.Value;
        }

        if (node is BinaryExpressionSyntax b)
        {
            Int32 left = EvaluateExpression(b.Left);
            Int32 right = EvaluateExpression(b.Right);

            if (b.OperatorToken.Kind == SyntaxKind.PlusToken) { return left + right; }
            else if (b.OperatorToken.Kind == SyntaxKind.MinusToken) { return left - right; }
            else if (b.OperatorToken.Kind == SyntaxKind.StarToken) { return left * right; }
            else if (b.OperatorToken.Kind == SyntaxKind.SlashToken) { return left / right; }
            else { throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}"); }
        }

        if (node is ParenthesizedExpressionSyntax p) { return EvaluateExpression(p.Expression); }
    
        throw new Exception($"Unexpected node {node.Kind}");
    }
}


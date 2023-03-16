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

        if (node is UnaryExpressionSyntax u)
        {
            Int32 operand = EvaluateExpression(u.Operand);

            return u.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => operand,
                SyntaxKind.MinusToken => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}")
            };
        }

        if (node is BinaryExpressionSyntax b)
        {
            Int32 left = EvaluateExpression(b.Left);
            Int32 right = EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                _ => throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}")
            };
        }

        if (node is ParenthesizedExpressionSyntax p) { return EvaluateExpression(p.Expression); }
    
        throw new Exception($"Unexpected node {node.Kind}");
    }
}


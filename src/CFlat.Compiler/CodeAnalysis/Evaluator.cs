using System;
using CFlat.Compiler.CodeAnalysis.Binding;
using CFlat.Compiler.CodeAnalysis.Enums;
using CFlat.Compiler.CodeAnalysis.Syntax;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis;

public sealed class Evaluator
{
    private readonly BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        _root = root;
    }

    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private object EvaluateExpression(BoundExpression node)
    {
        if (node is BoundLiteralExpression n)
        {
            return n.Value;
        }

        if (node is BoundUnaryExpression u)
        {
            Int32 operand = (Int32)EvaluateExpression(u.Operand);

            return u.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => operand,
                BoundUnaryOperatorKind.Negation => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
            };
        }

        if (node is BoundBinaryExpression b)
        {
            Int32 left = (Int32)EvaluateExpression(b.Left);
            Int32 right = (Int32)EvaluateExpression(b.Right);

            return b.OperatorKind switch
            {
                BoundBinaryOperatorKind.Addition => left + right,
                BoundBinaryOperatorKind.Subtraction => left - right,
                BoundBinaryOperatorKind.Multiplication => left * right,
                BoundBinaryOperatorKind.Division => left / right,
                _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
            };
        }
    
        throw new Exception($"Unexpected node {node.Kind}");
    }
}


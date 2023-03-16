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
            var operand = EvaluateExpression(u.Operand);

            return u.Op.Kind switch
            {
                BoundUnaryOperatorKind.Identity => (Int32)operand,
                BoundUnaryOperatorKind.Negation => -(Int32)operand,
                BoundUnaryOperatorKind.LogicalNegation => !(Boolean)operand,
                _ => throw new Exception($"Unexpected unary operator {u.Op}")
            };
        }

        if (node is BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.Op.Kind switch
            {
                BoundBinaryOperatorKind.Addition => (Int32)left + (Int32)right,
                BoundBinaryOperatorKind.Subtraction => (Int32)left - (Int32)right,
                BoundBinaryOperatorKind.Multiplication => (Int32)left * (Int32)right,
                BoundBinaryOperatorKind.Division => (Int32)left / (Int32)right,
                BoundBinaryOperatorKind.LogicalAnd => (Boolean)left && (Boolean)right,
                BoundBinaryOperatorKind.LogicalOr => (Boolean)left || (Boolean)right,
                _ => throw new Exception($"Unexpected binary operator {b.Op}")
            };
        }
    
        throw new Exception($"Unexpected node {node.Kind}");
    }
}


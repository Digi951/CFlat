﻿using System;
using CFlat.Compiler.CodeAnalysis.Enums;
using CFlat.Compiler.CodeAnalysis.Syntax;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Binding;

public sealed class Binder
{
    private readonly List<String> _diagnostics = new();

    public IEnumerable<String> Diagnostics => _diagnostics;

    public BoundExpression BindExpression(ExpressionSyntax syntax)
    {
        return syntax.Kind switch
        {
            SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
            SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
            SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
            SyntaxKind.ParenthesizedExpression => BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
            _ => throw new Exception($"Unexprected syntax {syntax.Kind}")
        };
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
    {
        BoundExpression boundLeft = BindExpression(syntax.Left);
        BoundExpression boundRight = BindExpression(syntax.Right);
        var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

        if (boundOperator is null)
        {
            _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
            return boundLeft;
        }

        return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
    }       

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
    {        
        var value = syntax.Value ?? 0;
        return new BoundLiteralExpression(value);
    }

    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
        BoundExpression boundOperand = BindExpression(syntax.Operand);
        var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

        if (boundOperator is null )
        {
            _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
            return boundOperand;
        }

        return new BoundUnaryExpression(boundOperator, boundOperand);
    }    
}


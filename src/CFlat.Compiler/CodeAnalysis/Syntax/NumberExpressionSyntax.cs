﻿using System;
using CFlat.Compiler.Enums;
using CFlat.Compiler.CodeAnalysis.Syntax;

namespace CFlat.Compiler;

sealed class NumberExpressionSyntax : ExpressionSyntax
{
    public NumberExpressionSyntax(SyntaxToken numberToken)
    {
        NumberToken = numberToken;
    }

    public override SyntaxKind Kind => SyntaxKind.NumberExpression;

    public SyntaxToken NumberToken { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NumberToken;
    }
}

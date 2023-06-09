﻿using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

public sealed class SyntaxToken : SyntaxNode
{
    public SyntaxToken(SyntaxKind kind, Int32 position, String? text = null, object? value = null)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }

    public override SyntaxKind Kind { get; }
    public int Position { get; }
    public string? Text { get; }
    public object? Value { get; }
    public TextSpan Span => new TextSpan(Position, Text.Length);
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }
}
﻿using System;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

public sealed class SyntaxTree
{
    public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
    {
        Diagnostics = diagnostics.ToArray();
        Root = root;
        EndOfFileToken = endOfFileToken;
    }

    public IReadOnlyList<Diagnostic> Diagnostics { get; }
    public ExpressionSyntax Root { get; }
    public SyntaxToken EndOfFileToken { get; }

    public static SyntaxTree Parse(String text)
    {
        Parser parser = new (text);
        return parser.Parse();
    }
}


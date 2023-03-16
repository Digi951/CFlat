using System;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Extensions;

internal static class SyntaxFacts
{
    public static Int32 GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.PlusToken => 5,
            SyntaxKind.MinusToken => 5,
            SyntaxKind.BangToken => 5,
            _ => 0
        };
    }

    public static Int32 GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.StarToken => 4,
            SyntaxKind.SlashToken => 4,
            SyntaxKind.PlusToken => 3,
            SyntaxKind.MinusToken => 3,
            SyntaxKind.AmpersandAmpersandToken => 2,
            SyntaxKind.PipePipeToken => 1,
            _ => 0
        };
    }

    public static SyntaxKind GetKeywordKind(string text)
    {
        return text switch
        {
            "true" => SyntaxKind.TrueKeyword,
            "false" => SyntaxKind.FalseKeyword,
            _ => SyntaxKind.IdentifierToken
        };
    }
}


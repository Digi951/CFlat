using System;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private List<String> _diagnostics = new();
    private readonly SyntaxToken[] _tokens;
    private Int32 _position;

    public Parser(String text)
    {
        List<SyntaxToken> tokens = new();
        Lexer lexer = new(text);

        SyntaxToken token;

        do
        {
            token = lexer.Lex();

            if (token.Kind is (not SyntaxKind.WhitespaceToken and
                                not SyntaxKind.BadToken))
            {
                tokens.Add(token);
            }

        } while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    public IEnumerable<String> Diagnostics => _diagnostics;

    private SyntaxToken Peek(Int32 offset)
    {
        Int32 index = _position + offset;

        if (index >= _tokens.Length) { return _tokens[_tokens.Length - 1]; }

        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;

        return current;
    }

    private SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.Kind == kind)
        {
            return NextToken();
        }

        _diagnostics.Add($"ERROR: Unexpexted token <{Current.Kind}>, expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null, null);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseTermExpression();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public SyntaxTree Parse()
    {
        ExpressionSyntax expression = ParseTermExpression();
        SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private ExpressionSyntax ParseTermExpression()
    {
        var left = ParseFactorExpression();

        while (Current.Kind == SyntaxKind.PlusToken ||
            Current.Kind == SyntaxKind.MinusToken)
        {
            var operatorToken = NextToken();
            var right = ParseFactorExpression();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParseFactorExpression()
    {
        var left = ParsePrimaryExpression();

        while (Current.Kind == SyntaxKind.StarToken ||
            Current.Kind == SyntaxKind.SlashToken)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(numberToken);
    }
}


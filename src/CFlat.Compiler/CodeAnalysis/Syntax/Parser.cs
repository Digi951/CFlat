﻿using System;
using CFlat.Compiler.Enums;
using CFlat.Compiler.CodeAnalysis.Extensions;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private DiagnosticBag _diagnostics = new();
    private readonly SyntaxToken[] _tokens;
    private Int32 _position;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    public Parser(String text)
    {
        List<SyntaxToken> tokens = new();
        Lexer lexer = new(text);

        SyntaxToken token;

        do
        {
            token = lexer.Lex();

            if (token.Kind is not SyntaxKind.WhitespaceToken &&
                token.Kind is not SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }

        } while (token.Kind != SyntaxKind.EndOfFileToken);

        _tokens = tokens.ToArray();
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    public DiagnosticBag Diagnostics => _diagnostics;

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

        _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
        return new SyntaxToken(kind, Current.Position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public SyntaxTree Parse()
    {
        ExpressionSyntax expression = ParseExpression();
        SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_diagnostics, expression, endOfFileToken);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Peek(0).Kind == SyntaxKind.IdentifierToken &&
            Peek(1).Kind == SyntaxKind.EqualsToken)
        {
            SyntaxToken identifierToken = NextToken();
            SyntaxToken operatorToken = NextToken();
            ExpressionSyntax right = ParseAssignmentExpression();
            return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }

    private ExpressionSyntax ParseBinaryExpression(Int32 parentPrecedence = 0)
    {
        ExpressionSyntax left;

        Int32 unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            SyntaxToken operatorToken = NextToken();
            ExpressionSyntax operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }

        while (true)
        {
            Int32 precedence = Current.Kind.GetBinaryOperatorPrecedence(); ;

            if (precedence == 0 || precedence <= parentPrecedence) { break; }

            SyntaxToken operatorToken = NextToken();
            ExpressionSyntax right = ParseBinaryExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }    

    private ExpressionSyntax ParsePrimaryExpression()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.OpenParenthesisToken:
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            case SyntaxKind.TrueKeyword:
            case SyntaxKind.FalseKeyword:
            {
                SyntaxToken keywordToken = NextToken();
                bool value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                return new LiteralExpressionSyntax(keywordToken, value);
            }
            case SyntaxKind.IdentifierToken:
            {
                var identifierToken = NextToken(); 
                return new NameExpressionSyntax(identifierToken);
            }        
            default:
            {
                var numberToken = MatchToken(SyntaxKind.NumberToken);
                return new LiteralExpressionSyntax(numberToken);
            }
        }
    }
}

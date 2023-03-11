using System;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

public class Lexer
{
    private readonly String _text;
    private Int32 _position;
    private List<String> _diagnostics = new();

    public Lexer(String text)
    {
        _text = text;
    }

    private Char Current
    {
        get
        {
            return _position >= _text.Length ? '\0' : _text[_position];
        }
    }

    public IEnumerable<String> Diagnostics => _diagnostics;

    public void Next()
    {
        _position++;
    }

    public SyntaxToken NextToken()
    {
        // <numbers>
        // + - * / ( )
        // <whitespace

        if (_position >= _text.Length)
        {
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0");
        }

        if (Char.IsDigit(Current))
        {
            Int32 start = _position;

            while (Char.IsDigit(Current))
            {
                Next();
            }

            Int32 length = _position - start;
            String text = _text.Substring(start, length);
            if (!Int32.TryParse(text, out var value))
            {
                _diagnostics.Add($"The number {_text} isn't a valid Int32.");
            }
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            Int32 start = _position;

            while (Char.IsWhiteSpace(Current))
            {
                Next();
            }

            Int32 length = _position - start;
            String text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text);
        }

        switch (Current)
        {
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+");
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-");
            case '*':
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*");
            case '/':
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/");
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(");
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")");
            default:
                _diagnostics.Add($"ERROR: Bad character input: {Current}");
                return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1));
        }
    }
}


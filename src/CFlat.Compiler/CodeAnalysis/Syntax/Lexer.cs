using System;
using CFlat.Compiler.CodeAnalysis.Extensions;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Syntax;

internal sealed class Lexer
{
    private readonly String _text;
    private Int32 _position;
    private DiagnosticBag _diagnostics = new();

    public Lexer(String text)
    {
        _text = text;
    }

    private Char Current => Peek(0);
    private Char Lookahed => Peek(1);
    

    private char Peek(Int32 offset)
    {
        var index = _position + offset;

        return index >= _text.Length
                ? '\0'
                : _text[index];
    }

    public DiagnosticBag Diagnostics => _diagnostics;

    public void Next()
    {
        _position++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public SyntaxToken Lex()
    {  
        if (_position >= _text.Length)
        {
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0");
        }

        var start = _position;

        if (Char.IsDigit(Current))
        {
            while (Char.IsDigit(Current))
            {
                Next();
            }

            Int32 length = _position - start;
            String text = _text.Substring(start, length);
            if (!Int32.TryParse(text, out var value))
            {
                _diagnostics. ReportInvalidNumber(new TextSpan(start, length), _text, typeof(Int32));
            }
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            while (Char.IsWhiteSpace(Current))
            {
                Next();
            }

            Int32 length = _position - start;
            String text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text);
        }

        if (Char.IsLetter(Current))
        {
            while (Char.IsLetter(Current))
            {
                Next();
            }

            Int32 length = _position - start;
            String text = _text.Substring(start, length);
            var kind = SyntaxFacts.GetKeywordKind(text);
            return new SyntaxToken(kind, start, text);
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
            case '&':
                if (Lookahed == '&')
                {
                    _position += 2;
                    return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&");
                }
                break;
            case '|':
                if (Lookahed == '|')
                {
                    _position += 2;
                    return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||");
                }
                break;
            case '=':
                if (Lookahed == '=')
                {
                    _position += 2;
                    return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==");
                }
                break;
            case '!':
                if (Lookahed == '=')
                {
                    _position += 2;
                    return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=");
                }
                else
                {
                    _position ++;
                    return new SyntaxToken(SyntaxKind.BangToken, start, "!");
                }
        }

        _diagnostics.ReportBadCharacter(_position, Current);
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1));
    }
}


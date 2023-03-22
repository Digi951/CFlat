namespace CFlat.Compiler.Enums;

public enum SyntaxKind
{
    // Tokens
    BadToken,
    EndOfFileToken,
    WhitespaceToken,
    NumberToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    BangToken,
    EqualsToken,
    EqualsEqualsToken,
    BangEqualsToken,
    AmpersandAmpersandToken,
    PipePipeToken,
    OpenParenthesisToken,
    CloseParenthesisToken,

    // Keywords
    TrueKeyword,
    FalseKeyword,
    IdentifierToken,

    // Expressions
    LiteralExpression,
    AssignmentExpression,
    NameExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
}


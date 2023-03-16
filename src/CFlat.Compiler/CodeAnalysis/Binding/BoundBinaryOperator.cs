using CFlat.Compiler.CodeAnalysis.Enums;
using CFlat.Compiler.Enums;

namespace CFlat.Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
        : this(syntaxKind, kind, type, type, type)
    {

    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundBinaryOperatorKind Kind { get; }
    public Type LeftType { get; }
    public Type RightType { get; }
    public Type ResultType { get; }
    

    private static BoundBinaryOperator[] _operators =
    {
        new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(Int32)),
        new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(Int32)),
        new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(Int32)),
        new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(Int32)),

        new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(Boolean)),
        new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(Boolean)),
    };


    public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType) { return op; }
        }

        return null;
    }
}

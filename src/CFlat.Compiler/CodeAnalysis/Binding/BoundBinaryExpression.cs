using CFlat.Compiler.CodeAnalysis.Enums;

namespace CFlat.Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right)
    {
        OperatorKind = operatorKind;
        Right = right;
        Left = left;
    }

    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public override Type Type => Left.Type;

    public BoundExpression Left { get; }
    public BoundBinaryOperatorKind OperatorKind { get; }
    public BoundExpression Right { get; }
}

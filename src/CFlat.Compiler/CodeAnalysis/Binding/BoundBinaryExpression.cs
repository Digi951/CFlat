using CFlat.Compiler.CodeAnalysis.Enums;

namespace CFlat.Compiler.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right)
    {
        Op = op;
        Right = right;
        Left = left;
    }

    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public override Type Type => Left.Type;

    public BoundExpression Left { get; }
    public BoundBinaryOperator Op { get; }
    public BoundExpression Right { get; }
}

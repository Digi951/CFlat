using CFlat.Compiler.CodeAnalysis.Enums;

namespace CFlat.Compiler.CodeAnalysis.Binding;

public abstract class BoundNode
{
    public abstract BoundNodeKind Kind { get; }
    
}


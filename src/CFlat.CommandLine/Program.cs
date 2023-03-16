using CFlat.Compiler.CodeAnalysis.Syntax;
using CFlat.Compiler.CodeAnalysis;
using CFlat.Compiler.CodeAnalysis.Binding;

Boolean showTree = false;

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();

    if (String.IsNullOrWhiteSpace(line)) { return; }

    if (line == "#showTree")
    {
        showTree = !showTree;
        Console.WriteLine(showTree ? "Showing parse trees" : "Not showing parse trees");
        continue;
    }
    else if (line == "#cls")
    {
        Console.Clear();
        continue;
    }

    var syntaxTree = SyntaxTree.Parse(line);
    var binder = new Binder();
    var boundExpression = binder.BindExpression(syntaxTree.Root);

    IReadOnlyList<String> diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }


    if (!diagnostics.Any())
    {
        Evaluator evaluator = new(boundExpression);
        Int32 result = evaluator.Evaluate();
        Console.WriteLine(result);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;

        foreach (var diagnostic in syntaxTree.Diagnostics)
        {
            Console.WriteLine(diagnostic);
        }

        Console.ResetColor();
    }
}

void PrettyPrint(SyntaxNode node, String indent = "", Boolean isLast = true)
{
    // └──
    // ├──
    // │

    var marker = isLast ? "└──" : "├──";

    Console.Write(indent);
    Console.Write(marker);
    Console.Write(node.Kind);

    if (node is SyntaxToken t && t.Value is not null)
    {
        
        Console.Write(" ");
        Console.Write(t.Value);
    }

    Console.WriteLine();

    indent += isLast ? "   " : "│   ";

    SyntaxNode ? lastChild = node.GetChildren().LastOrDefault();

    foreach (var child in node.GetChildren())
    {
        PrettyPrint(child, indent, child == lastChild);
    }
}
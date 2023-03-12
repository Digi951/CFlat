using CFlat.Compiler;
using CFlat.Compiler.Enums;
using CFlat.Compiler.CodeAnalysis.Syntax;

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

    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }    

    if (!syntaxTree.Diagnostics.Any())
    {
        Evaluator evaluator = new(syntaxTree.Root);
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
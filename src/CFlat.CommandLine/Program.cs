using CFlat.Compiler.CodeAnalysis.Syntax;
using CFlat.Compiler.CodeAnalysis;
using CFlat.Compiler.CodeAnalysis.Binding;

Boolean showTree = false;
var variables = new Dictionary<VariableSymbol, Object>();

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
    var compilation = new Compilation(syntaxTree);
    var result = compilation.Evaluate(variables);

    var diagnostics = result.Diagnostics;

    if (showTree)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrettyPrint(syntaxTree.Root);
        Console.ResetColor();
    }

    if (!diagnostics.Any())
    {        
        Console.WriteLine(result.Value);
    }
    else
    {
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(diagnostic);
            Console.ResetColor();

            String prefix = line.Substring(0, diagnostic.Span.Start);
            String error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
            String suffix = line.Substring(diagnostic.Span.End);

            Console.Write("    ");
            Console.Write(prefix);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(error);
            Console.ResetColor();

            Console.Write(suffix);
            Console.WriteLine();
        }

        Console.WriteLine();
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
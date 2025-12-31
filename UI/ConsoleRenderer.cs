using System.Text;

public static class ConsoleRenderer
{
    public static void DrawHeader(string title, ConsoleColor color)
    {
        string border = new('═', title.Length + 8);
        
        ColorPrinter.PrintColor($"\n╔═{border}═╗", color);
        ColorPrinter.PrintColor($"║     {title}     ║", color);
        ColorPrinter.PrintColor($"╚═{border}═╝\n", color);
    }
}
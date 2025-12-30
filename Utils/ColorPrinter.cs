
public static class ColorPrinter
{
    public static void PrintColor(string text, ConsoleColor color, bool makeEnter = true)
    {
        Console.ForegroundColor = color;
        
        if (makeEnter == true)
        {
            Console.WriteLine(text);
            Console.ResetColor();
            return;
        }

        Console.Write(text);
        Console.ResetColor();
    }
}
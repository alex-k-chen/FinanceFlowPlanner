public static class MotivationService
{
    public static void ShowMotivation()
    {
        string[] quotes = 
        {
            "💰 Every euro in your account is a step towards your dream!",
            "🎯 You're on the right track!",
            "💪 Financial discipline is a superpower!",
            "📈 Small savings grow into big opportunities!",
            "🔥 Consistency beats intensity in finances!",
            "💎 Smart spending today = freedom tomorrow!"
        };
        ColorPrinter.PrintColor(quotes[new Random().Next(quotes.Length)], ConsoleColor.Cyan);
    }
}
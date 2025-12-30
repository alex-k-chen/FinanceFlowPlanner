public static class GoalManager
{
    public static void AddFinancialGoal()
    {
        Console.Write("Enter goal description: ");
        string? goalName = Console.ReadLine();

        Console.Write("Enter target sum: ");
        string? amountInput = Console.ReadLine();

        Console.Write("Deadline (YYYY-MM-DD) or Enter for none: ");
        string? deadlineInput = Console.ReadLine();

        DateTime? deadline = null;

        if (!string.IsNullOrWhiteSpace(deadlineInput))
        {
            if (!DateTime.TryParse(deadlineInput, out DateTime parsedDeadline))
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Invalid deadline format!", ConsoleColor.Red);
                return;
            }

            deadline = parsedDeadline;
        }
        if (decimal.TryParse(amountInput, out decimal amount))
        {
            FinancialGoal goal = new(
                Name: string.IsNullOrEmpty(goalName) ? "Unnamed" : goalName,
                TargetAmount: amount,
                Deadline: deadline
            );

            Program.goals.Add(goal);
            JsonDataService.SaveData(Program.goals, Program.expenses);
            Console.Clear();
            ColorPrinter.PrintColor("✅ Goal was added!", ConsoleColor.Green);
        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: The sum must be number!", ConsoleColor.Red);
        }
    }
}
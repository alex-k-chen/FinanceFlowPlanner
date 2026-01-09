public class RecommendationEngine
{
    public static void GenerateTips(List<FinancialGoal> goals, List<Expense> expenses) {
        if (goals.Count == 0)
        {
            Console.WriteLine("🎯 Set your first financial goal!");
        }

        foreach (var expense in expenses)
        {
            if (expense.Amount > 250)
            {
                Console.WriteLine($"💸 Big expense: {expense.Description} -{expense.Amount:C}");
            }
        }

        Console.WriteLine();

        foreach (var goal in goals)
        {
            if (goal.CurrentAmount == 0)
            {
                Console.WriteLine($"⏳ '{goal.Name}' has no savings yet!");
            }
            
            if (!goal.Deadline.HasValue)
            {
                ColorPrinter.PrintColor($"⚠️ Goal '{goal.Name}' has no deadline set!\n", ConsoleColor.Yellow);
                continue;
            }

            TimeSpan daysLeft = goal.Deadline.Value - DateTime.Now;

            if (daysLeft.Days < 100)
            {
                decimal dailyNeeded = goal.RemainingAmount / daysLeft.Days;

                if (dailyNeeded > 0)
                {
                    Console.WriteLine($"‼️ You have only {daysLeft.Days} days left to achive your goal: '{goal.Name}'!");
                    Console.WriteLine($"💡 You need to save {dailyNeeded:C} daily to achive this goal!");
                    Console.WriteLine();
                }
            }
        }
    }
}
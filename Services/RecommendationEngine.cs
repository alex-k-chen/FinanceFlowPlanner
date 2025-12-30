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

        foreach (var goal in goals)
        {
            if (goal.CurrentAmount == 0)
            {
                Console.WriteLine($"⏳ Start saving for: {goal.Name}");
            }
        }
    }
}
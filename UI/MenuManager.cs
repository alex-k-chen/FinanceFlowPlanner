
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

public static class MenuManager
{
    public static void ShowMainMenu()
    {
        ConsoleRenderer.DrawHeader("💸 FINANCE FLOW PLANNER", ConsoleColor.Green);
        Console.WriteLine("MAIN MENU:");
        Console.WriteLine("1. 📝 Add financial goal");
        Console.WriteLine("2. 🎯 View goals");
        Console.WriteLine("3. 💸 Add expense");
        Console.WriteLine("4. 📈 View expenses");
        Console.WriteLine("5. 💰 Manage budgets");
        Console.WriteLine("6. 📊 Show analytics");
        Console.WriteLine("0. 🚪 Exit");
        Console.Write("\nChoose option: ");
    }

    public static void ShowGoals()
    {
        Console.Clear();
        ConsoleRenderer.DrawHeader("     📋 GOALS LIST     ", ConsoleColor.DarkBlue);

        if (Program.goals.Count == 0)
        {
            Console.Clear();
            ColorPrinter.PrintColor("⚠️ Goals list is empty!", ConsoleColor.Yellow);
        }
        else
        {
            for (int i = 0; i < Program.goals.Count; i++)
            {
                Console.WriteLine($"┌─[{i + 1}]─ {Program.goals[i].Name}");
                Console.WriteLine($"│   Target: {Program.goals[i].TargetAmount:C}");
                Console.WriteLine($"│   Progress: {Program.goals[i].CurrentAmount:C} / {Program.goals[i].TargetAmount:C}");
                Console.WriteLine($"│   Remaining: {Program.goals[i].RemainingAmount:C}");
                Console.WriteLine($"│   Deadline: {Program.goals[i].DeadlineDisplay}");
                Console.WriteLine($"└─────────────────────────────────────");
            }

            Console.WriteLine("\nMENU:");
            Console.WriteLine("1. Add money");
            Console.WriteLine("2. Edit goal");
            Console.WriteLine("3. Main menu");
            Console.Write("\nChoose option: ");
            string? optionInput = Console.ReadLine();

            if (int.TryParse(optionInput, out int option))
            {
                switch(option)
                {
                    case 1:
                        GoalManager.AddMoneyToGoal();
                        break;
                    case 2:
                        GoalManager.EditGoal();
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        ColorPrinter.PrintColor("⚠️ Wrong menu index!", ConsoleColor.Yellow);
                        break;
                }
            }
        }
    }

    public static void ShowExpenses()
    {
        decimal TotalExpensesSum = 0;

        Console.Clear();
        ConsoleRenderer.DrawHeader("    📋 EXPENSES LIST    ", ConsoleColor.DarkBlue);

        if (Program.expenses.Count == 0)
        {
            Console.Clear();
            ColorPrinter.PrintColor("⚠️ Expenses list is empty!", ConsoleColor.Yellow);
        }
        else
        {
            for (int i = 0; i < Program.expenses.Count; i++)
            {
                Console.WriteLine($"┌─[{i + 1}]─ {Program.expenses[i].Description}");
                Console.WriteLine($"│   Category: {Program.expenses[i].Category}");
                Console.WriteLine($"│   Amount: {Program.expenses[i].Amount:C}");
                Console.WriteLine($"│   Date: {Program.expenses[i].DateDisplay}");
                Console.WriteLine($"└────────────────────────────────────");

                TotalExpensesSum += Program.expenses[i].Amount;
            }

            Console.Write("\n💰 Total spent: ");
            ColorPrinter.PrintColor($"{TotalExpensesSum:C}", ConsoleColor.DarkGreen);

            Console.WriteLine("\nMENU:");
            Console.WriteLine("1. Edit expense");
            Console.WriteLine("2. Main menu");
            Console.Write("\nChoose option: ");
            string? optionInput = Console.ReadLine();

            if (int.TryParse(optionInput, out int option))
            {
                switch(option)
                {
                    case 1:
                        ExpenseManager.EditExpense();
                        break;
                    case 2:
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        ColorPrinter.PrintColor("⚠️ Wrong menu index!", ConsoleColor.Yellow);
                        break;
                }
            }
        }
    }

    public static void ShowAnalytics()
    {
        Console.Clear();
        ConsoleRenderer.DrawHeader("              📊 ANALYTICS              ", ConsoleColor.Magenta);

        if (Program.expenses.Count == 0)
        {
            Console.Clear();
            ColorPrinter.PrintColor("⚠️ No expenses to analyze yet!", ConsoleColor.Yellow);
            return;
        }

        ExpenseCategory topCtg = ExpenseCategory.Other;
        List<(ExpenseCategory categoryName, decimal total)> overspentCategories = [];
        decimal maxAmount = 0;
        decimal minExpense = decimal.MaxValue;
        decimal maxExpense = 0;
        decimal allExpensesTotal = 0;
        decimal averageExpenseSum;

        foreach (var expense in Program.expenses)
        {
            allExpensesTotal += expense.Amount;
        }

        foreach (ExpenseCategory category in Enum.GetValues(typeof(ExpenseCategory)))
        {
            decimal categoryTotal = 0;
            bool overspent = false;

            foreach (var expense in Program.expenses)
            {
                if (expense.Category == category)
                {
                    categoryTotal += expense.Amount;
                }

                if (expense.Amount > maxExpense)
                {
                    maxExpense = expense.Amount;
                }

                if (expense.Amount < minExpense)
                {
                    minExpense = expense.Amount;
                }
            }

            if (categoryTotal > maxAmount)
            {
                maxAmount = categoryTotal;
                topCtg = category;
            }

            int barSymbolCount = (int)(categoryTotal / allExpensesTotal * 50);

            if (barSymbolCount > 30)
            {
                barSymbolCount = 30;
            }

            if (categoryTotal > Program.categoryBudgets[category])
            {
                overspent = true;
            }

            if (categoryTotal > 0)
            {
                decimal percentageCtg = categoryTotal / allExpensesTotal;
                Console.WriteLine($"{category, -15} {new string('█', barSymbolCount)} {categoryTotal:C} ({percentageCtg:P2})");
            }

            if (overspent == true)
            {
                overspentCategories.Add((category, categoryTotal));
            }
        }

        averageExpenseSum = allExpensesTotal / Program.expenses.Count;
        
        Console.WriteLine();
        RecommendationEngine.GenerateTips(Program.goals, Program.expenses);

        Console.WriteLine();
        foreach (var (categoryName, total) in overspentCategories)
        {
            Console.Write($"{categoryName} -> ");
            ColorPrinter.PrintColor("⚠️ Overspent budget!", ConsoleColor.Yellow, false);
            Console.WriteLine($" ({total:C} / {Program.categoryBudgets[categoryName]:C})");
        }

        Console.WriteLine($"\n🧾 Transactions: {Program.expenses.Count}");
        Console.WriteLine($"⚖️ Average: {averageExpenseSum:C}");
        Console.WriteLine($"🔺 Max spent: {maxExpense:C}");
        Console.WriteLine($"🔻 Min spent: {minExpense:C}");
        Console.Write("\n🏆 Top category: ");
        ColorPrinter.PrintColor($"{topCtg}", ConsoleColor.DarkYellow);
    }
}
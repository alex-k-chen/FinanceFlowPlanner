
public static class MenuManager
{
    public static void ShowMainMenu()
    {
        ColorPrinter.PrintColor("\n╔══════════════════════════════════════╗", ConsoleColor.Green);
        ColorPrinter.PrintColor("║       💸 FINANCE FLOW PLANNER        ║", ConsoleColor.Green);
        ColorPrinter.PrintColor("╚══════════════════════════════════════╝", ConsoleColor.Green);
        Console.WriteLine("\nMAIN MENU:");
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
        ColorPrinter.PrintColor("\n╔══════════════════════════════════════╗", ConsoleColor.DarkBlue);
        ColorPrinter.PrintColor("║            📋 GOALS LIST             ║", ConsoleColor.DarkBlue);
        ColorPrinter.PrintColor("╚══════════════════════════════════════╝\n", ConsoleColor.DarkBlue);

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
            Console.Write("\nAdd money to some goal? (y/n): ");
            string? add_money_input = Console.ReadLine();

            if (char.TryParse(add_money_input, out char add_money) && (add_money == 'y' || add_money == 'Y'))
            {
                Console.Write("Enter the goal number: ");
                string? goalNumInput = Console.ReadLine();

                if (int.TryParse(goalNumInput, out int goalNum))
                {
                    if (goalNum < 1 || goalNum > Program.goals.Count)
                    {
                        Console.Clear();
                        ColorPrinter.PrintColor("❌ Error: The goal doesn't exist!", ConsoleColor.Red);
                    }
                    else
                    {
                        Console.Write("Enter sum of money: ");
                        string? sumInput = Console.ReadLine();

                        if (decimal.TryParse(sumInput, out decimal sum) && sum > 0)
                        {
                            Program.goals[goalNum - 1] = Program.goals[goalNum - 1].AddMoney(sum);
                            JsonDataService.SaveData(Program.goals, Program.expenses);
                            Console.Clear();
                            ColorPrinter.PrintColor($"✅ {sum:C} was added!", ConsoleColor.Green);
                            return;
                        }
                        else
                        {
                            Console.Clear();
                            ColorPrinter.PrintColor("❌ Error: The sum must be a positive number!", ConsoleColor.Red);
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    ColorPrinter.PrintColor("❌ Error: Enter a number!", ConsoleColor.Red);
                }
            }
            else
            {
                Console.Clear();
            }
        }
    }

    public static void ShowExpenses()
    {
        decimal TotalExpensesSum = 0;

        Console.Clear();
        ColorPrinter.PrintColor("\n╔══════════════════════════════════════╗", ConsoleColor.DarkBlue);
        ColorPrinter.PrintColor("║           📋 EXPENSES LIST           ║", ConsoleColor.DarkBlue);
        ColorPrinter.PrintColor("╚══════════════════════════════════════╝\n", ConsoleColor.DarkBlue);

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
                Console.WriteLine($"└─────────────────────────────────────");

                TotalExpensesSum += Program.expenses[i].Amount;
            }

            Console.Write("\n💰 Total spent: ");
            ColorPrinter.PrintColor($"{TotalExpensesSum:C}", ConsoleColor.DarkGreen);
        }
    }

    public static void ShowAnalytics()
    {
        Console.Clear();
        ColorPrinter.PrintColor("\n╔══════════════════════════════════════╗", ConsoleColor.Magenta);
        ColorPrinter.PrintColor("║             📊 ANALYTICS             ║", ConsoleColor.Magenta);
        ColorPrinter.PrintColor("╚══════════════════════════════════════╝\n", ConsoleColor.Magenta);

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
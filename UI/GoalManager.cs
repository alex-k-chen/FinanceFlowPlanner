public static class GoalManager
{
    public static void AddMoneyToGoal()
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
                    JsonDataService.SaveFinanceData(Program.goals, Program.expenses);
                    Console.Clear();
                    ColorPrinter.PrintColor($"✅ {sum:C}", ConsoleColor.Green, false);
                    Console.WriteLine($" was added to '{Program.goals[goalNum - 1].Name}'!");
                    return;
                }
                else
                {
                    Console.Clear();
                    ColorPrinter.PrintColor("❌ Error: Wrong sum format!", ConsoleColor.Red);
                }
            }
        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
        }
    }

    public static void EditGoal()
    {
        Console.Write("Enter the goal number: ");
        string? goalNumInput = Console.ReadLine();

        if (int.TryParse(goalNumInput, out int goalNum))
        {
            if (goalNum > 0 && goalNum <= Program.goals.Count)
            {
                Console.WriteLine($"[{goalNum}] {Program.goals[goalNum - 1].Name}");

                Console.Write("Write new goal name: ");
                string? newGoalName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newGoalName))
                {
                    Program.goals[goalNum - 1] = Program.goals[goalNum - 1] with
                    {
                        Name = newGoalName
                    };

                    JsonDataService.SaveFinanceData(Program.goals, Program.expenses);
                    Console.Clear();
                    ColorPrinter.PrintColor("✅ Goal name changed!", ConsoleColor.Green);
                }
                else
                {
                    Console.Clear();
                    ColorPrinter.PrintColor("⚠️ Empty goal name!", ConsoleColor.Yellow);
                }
            }
            else
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Goal does not exist!", ConsoleColor.Red);
            }
        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
        }
    }

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
            JsonDataService.SaveFinanceData(Program.goals, Program.expenses);
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
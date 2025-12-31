public static class ExpenseManager
{
    public static void EditExpense()
    {
        Console.Write("Enter the expense number: ");
        string? expenseNumInput = Console.ReadLine();

        if (int.TryParse(expenseNumInput, out int expenseNum))
        {
            if (expenseNum > 0 && expenseNum <= Program.expenses.Count)
            {
                Console.WriteLine($"[{expenseNum}] {Program.expenses[expenseNum - 1].Description}");
                Console.Write("Enter new expense description: ");
                string? newExpenseDesc = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newExpenseDesc))
                {
                    Program.expenses[expenseNum - 1] = Program.expenses[expenseNum - 1] with
                    {
                        Description = newExpenseDesc
                    };

                    JsonDataService.SaveFinanceData(Program.goals, Program.expenses);
                    Console.Clear();
                    ColorPrinter.PrintColor("✅ Expense description changed!", ConsoleColor.Green);
                }
                else
                {
                    Console.Clear();
                    ColorPrinter.PrintColor("⚠️ Empty expense description!", ConsoleColor.Yellow);
                }       
            }
            else
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Expense does not exist!", ConsoleColor.Red);
            }

        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
        }
    }

    public static void AddExpense()
    {
        Console.Write("Enter expense description: ");
        string? expenseDesc = Console.ReadLine();

        Console.WriteLine("Categories: 1.Food 2.Transport 3.Entertainment 4.Bills 5.Shopping 6.Health 7.Other");
        Console.Write("Choose category (1-7): ");
        string? categoryInput = Console.ReadLine();

        Console.Write("Enter amount: ");
        string? amountInput = Console.ReadLine();

        Console.Write("Enter date (YYYY-MM-DD) or Enter for today: ");
        string? dateInput = Console.ReadLine();

        DateTime? date = null;

        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (!DateTime.TryParse(dateInput, out DateTime parsedDate))
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Invalid date format!", ConsoleColor.Red);
                return;
            }

            date = parsedDate;
        }
        if (int.TryParse(categoryInput, out int categoryNum) && (categoryNum >=1) && (categoryNum <=7) &&
        decimal.TryParse(amountInput, out decimal amount) && amount > 0)
        {
            Expense expense = new(
                Description: string.IsNullOrEmpty(expenseDesc) ? "Unnamed" : expenseDesc,
                Category: (ExpenseCategory)(categoryNum - 1),
                Amount: amount,
                Date: date
            );

            Program.expenses.Add(expense);
            JsonDataService.SaveFinanceData(Program.goals, Program.expenses);
            Console.Clear();
            ColorPrinter.PrintColor("✅ Expense was added!", ConsoleColor.Green);
        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Invalid category or amount input!", ConsoleColor.Red);
        }
    }
}
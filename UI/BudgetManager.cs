public static class BudgetManager
{
    static void EditBudget()
    {
        Console.WriteLine("BUDGET EDIT");
        Console.Write("Enter number of category: ");
        string? ctgNumberInput = Console.ReadLine();

        if (int.TryParse(ctgNumberInput, out int ctgNumber))
        {
            Console.Write("Enter new category budget: ");
            string? newBudgetInput = Console.ReadLine();

            if (decimal.TryParse(newBudgetInput, out decimal newBudget))
            {
                ExpenseCategory categoryName = (ExpenseCategory)(ctgNumber - 1);

                Program.categoryBudgets[categoryName] = newBudget;
                JsonDataService.SaveBudgets(Program.categoryBudgets);
                Console.Clear();
                Console.WriteLine($"✅ Budget of category '{categoryName}' changed to {newBudget:C}");
            }
            else
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Wrong input format", ConsoleColor.Red);
            }
        }
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
        }
    }
    
    public static void ManageBudgets()
    {
        Console.Clear();
        ConsoleRenderer.DrawHeader("            💰 MANAGE BUDGETS            ", ConsoleColor.Blue);

        List<(ExpenseCategory categoryName, decimal total)> allCategories = [];

        foreach (ExpenseCategory category in Enum.GetValues(typeof(ExpenseCategory)))
        {
            decimal categoryTotal = 0;

            foreach (var expense in Program.expenses)
            {
                if (expense.Category == category)
                {
                    categoryTotal += expense.Amount;
                }
            }

            allCategories.Add((category, categoryTotal));
        }

        Console.WriteLine($"{"Category", -15} {"Spent", 9} {"Budget", 13} {"Status", 10}");
        Console.WriteLine(new string('─', 54));

        foreach (KeyValuePair<ExpenseCategory, decimal> categoryBudget in Program.categoryBudgets)
        {
            foreach (var (categoryName, total) in allCategories)
            {
                if (categoryBudget.Key == categoryName)
                {
                    int index = (int)categoryBudget.Key + 1;

                    if (total > categoryBudget.Value)
                    {
                        Console.Write($"[{index}] {categoryBudget.Key, -15} ");
                        ColorPrinter.PrintColor($"{total, 10:C}", ConsoleColor.Red, false);
                        Console.Write($" / {categoryBudget.Value, 10:C} ");
                        ColorPrinter.PrintColor("⚠️ OVERS", ConsoleColor.Red);
                    }
                    else
                    {
                        Console.Write($"[{index}] {categoryBudget.Key, -15} {total, 10:C} / {categoryBudget.Value, 10:C} ");
                        ColorPrinter.PrintColor("✅ OK", ConsoleColor.Green);
                    }
                }
            }
        }

        Console.WriteLine("\nMENU:");
        Console.WriteLine("1. Edit budget");
        Console.WriteLine("2. Main menu");
        Console.Write("\nChoose option: ");
        string? optionInput = Console.ReadLine();

        if (int.TryParse(optionInput, out int option))
        {
            switch(option)
            {
                case 1:
                    EditBudget();
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
        else
        {
            Console.Clear();
            ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
        }
    }
}
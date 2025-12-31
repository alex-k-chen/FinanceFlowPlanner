using System.Globalization;

class Program
{
    // Application global state - accessible from all parts of the program
    public static Dictionary<ExpenseCategory, decimal> categoryBudgets = [];
    public static List<FinancialGoal> goals = [];
    public static List<Expense> expenses = [];

    /// <summary>
    /// Main entry point of the Finance Flow Planner application
    /// Responsible for initialization, main loop, and cleanup
    /// </summary>
    static void Main()
    {
        // Set German culture for consistent currency formatting (€ symbol, decimal separators)
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");

        Console.Clear();  // Start with clean console
        Console.WriteLine();  // Empty line for better visual spacing
        
        // Show motivational quote to encourage users
        MotivationService.ShowMotivation();

        // Load persisted data from JSON files
        var (loadedGoals, loadedExpenses) = JsonDataService.LoadData();
        goals = loadedGoals;
        expenses = loadedExpenses;
        
        // Load category budgets (separate from main data)
        categoryBudgets = JsonDataService.LoadBudgets();

        // Main application loop - runs until user chooses to exit
        while (true)
        {
            // Display main menu with all available options
            MenuManager.ShowMainMenu();
            string? choiceInput = Console.ReadLine();

            // Validate and parse user input
            if (int.TryParse(choiceInput, out int choice))
            {
                // Route user choice to appropriate functionality
                switch (choice)
                {
                    case 0:  // Exit application
                        JsonDataService.SaveFinanceData(goals, expenses);  // Persist before exit
                        Console.WriteLine("Closing program...");
                        return;  // Exit Main() method, ending the program
                        
                    case 1:  // Add new financial goal
                        GoalManager.AddFinancialGoal();
                        break;
                        
                    case 2:  // View existing goals
                        MenuManager.ShowGoals();
                        break;
                        
                    case 3:  // Add new expense
                        ExpenseManager.AddExpense();
                        break;
                        
                    case 4:  // View expense history
                        MenuManager.ShowExpenses();
                        break;
                        
                    case 5:  // Manage category budgets
                        BudgetManager.ManageBudgets();
                        break;
                        
                    case 6:  // Show spending analytics and insights
                        MenuManager.ShowAnalytics();
                        break;
                        
                    default:  // Invalid menu option
                        Console.Clear();
                        ColorPrinter.PrintColor("⚠️ Wrong menu index!", ConsoleColor.Yellow);
                        break;
                }
            }
            else  // Non-numeric input
            {
                Console.Clear();
                ColorPrinter.PrintColor("❌ Error: Wrong input format!", ConsoleColor.Red);
            }
        }
    }
}
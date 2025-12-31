using System.Text.Json;

public static class JsonDataService
{
    private const string FileName = "finance_data.json";
    private const string FileNameBudgets = "budgets.json";

    public static void SaveFinanceData(List<FinancialGoal> goals, List<Expense> expenses)
    {
        try
        {
            FinanceData data = new(
                Version: "1.0",
                LastSaved: null,
                Goals: goals,
                Expenses: expenses
            );

            string DataSave = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreReadOnlyProperties = true
            });

            File.WriteAllText(FileName, DataSave);   
        }
        catch (Exception ex)
        {
            ColorPrinter.PrintColor($"⚠️ Error saving data: {ex.Message}", ConsoleColor.Red);
        }
    }

    public static (List<FinancialGoal>, List<Expense>) LoadData()
    {
        if (!File.Exists(FileName))
        {
            return ([], []);
        }
        
        try
        {
            string DataReadJson = File.ReadAllText(FileName);

            if (string.IsNullOrWhiteSpace(DataReadJson))
            {
                return ([], []);
            }

            var DataRead = JsonSerializer.Deserialize<FinanceData>(DataReadJson);

            return (DataRead?.Goals ?? [], DataRead?.Expenses ?? []);
        }
        catch (JsonException)
        {
            ColorPrinter.PrintColor("⚠️ Corrupted save file. Starting fresh!", ConsoleColor.Yellow);

            return ([], []);
        }
        catch (Exception ex)
        {
            ColorPrinter.PrintColor($"⚠️ Error loading data: {ex.Message}", ConsoleColor.Red);

            return ([], []);
        }
    }

    public static Dictionary<ExpenseCategory, decimal> GetDefaultBudgets()
    {
        return new()
        {
            { ExpenseCategory.Food, 400m },
            { ExpenseCategory.Transport, 200m },
            { ExpenseCategory.Entertainment, 150m },
            { ExpenseCategory.Bills, 1000m },
            { ExpenseCategory.Shopping, 250m },
            { ExpenseCategory.Health, 300m },
            { ExpenseCategory.Other, 100m }
        };
    }

    public static void SaveBudgets(Dictionary<ExpenseCategory, decimal> categoryBudgets)
    {
        try
        {
            string DataBudgetSave = JsonSerializer.Serialize(categoryBudgets, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreReadOnlyProperties = true
            });

            File.WriteAllText(FileNameBudgets, DataBudgetSave);
        }
        catch (Exception ex)
        {
            ColorPrinter.PrintColor($"⚠️ Error saving data: {ex.Message}", ConsoleColor.Red);
        }
    }

    public static Dictionary<ExpenseCategory, decimal> LoadBudgets()
    {
        if (!File.Exists(FileNameBudgets))
        {
            return GetDefaultBudgets();
        }

        try
        {
            string DataReadJson = File.ReadAllText(FileNameBudgets);

            var DataRead = JsonSerializer.Deserialize<Dictionary<ExpenseCategory, decimal>>(DataReadJson);

            return DataRead ?? GetDefaultBudgets();
        }
        catch (JsonException)
        {
            ColorPrinter.PrintColor("⚠️ Corrupted save file. Starting fresh!", ConsoleColor.Yellow);

            return GetDefaultBudgets();
        }
        catch (Exception ex)
        {
            ColorPrinter.PrintColor($"⚠️ Error loading data: {ex.Message}", ConsoleColor.Red);

            return GetDefaultBudgets();
        }
    }
}
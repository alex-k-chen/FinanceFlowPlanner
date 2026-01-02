using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PdfExportService
{
    public static void PdfExport()
    {
        if ((Program.goals == null || Program.goals.Count == 0) && 
            (Program.expenses == null || Program.expenses.Count == 0))
        {
            Console.WriteLine("⚠️ No data available for export!");
            return;
        }

        QuestPDF.Settings.License = LicenseType.Community;
        
        string fileName = $"FinanceReport_{DateTime.Now:dd_MM_yyyy}.pdf";
        decimal totalExpenses = Program.expenses?.Sum(e => e.Amount) ?? 0;
        decimal totalGoals = Program.goals?.Sum(g => g.TargetAmount) ?? 0;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                
                page.Header()
                    .PaddingBottom(20)
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text("FINANCE FLOW PLANNER")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);
                        
                        row.ConstantItem(120)
                            .AlignRight()
                            .Text($"{DateTime.Now:dd.MM.yyyy}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);
                    });
                
                page.Content()
                    .Column(column =>
                    {
                        // Title Section
                        column.Item()
                            .PaddingBottom(30)
                            .AlignCenter()
                            .Text("FINANCIAL REPORT")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        
                        // Summary Statistics
                        column.Item()
                            .PaddingBottom(30)
                            .Background(Colors.Grey.Lighten3)
                            .Padding(15)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Item().Text("Total Goals").FontSize(12).SemiBold();
                                        col.Item().Text($"{Program.goals?.Count ?? 0}").FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                                    });
                                
                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Item().Text("Total Expenses").FontSize(12).SemiBold();
                                        col.Item().Text($"{Program.expenses?.Count ?? 0}").FontSize(18).Bold().FontColor(Colors.Red.Darken2);
                                    });
                                
                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Item().Text("Expenses Total").FontSize(12).SemiBold();
                                        col.Item().Text($"{totalExpenses:C}").FontSize(18).Bold();
                                    });
                            });
                        
                        // Financial Goals Section
                        if (Program.goals?.Count > 0)
                        {
                            column.Item()
                                .PaddingBottom(10)
                                .Text("FINANCIAL GOALS")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);
                            
                            column.Item()
                                .PaddingBottom(30)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(30);  // №
                                        columns.ConstantColumn(160);    // Name
                                        columns.ConstantColumn(130);   // Target amount
                                        columns.RelativeColumn();    // Deadline
                                        columns.RelativeColumn();    // Status
                                    });
                                    
                                    // Table Header
                                    table.Header(header =>
                                    {
                                        header.Cell().Text("#").Bold();
                                        header.Cell().Text("Goal name").Bold();
                                        header.Cell().Text("Target amount").Bold();
                                        header.Cell().Text("Deadline").Bold();
                                        header.Cell().Text("Status").Bold().AlignRight();
                                    });
                                    
                                    // Financial Goals Data
                                    for (int i = 0; i < Program.goals.Count; i++)
                                    {
                                        var goal = Program.goals[i];
                                        bool isOverdue = goal.Deadline < DateTime.Now;

                                        table.Cell().Text($"{i + 1}").FontSize(10);
                                        table.Cell().Text(goal.Name).FontSize(11);
                                        table.Cell().Text($"{goal.TargetAmount:C}").FontSize(11);
                                        table.Cell().Text(goal.DeadlineDisplay)
                                            .FontColor(isOverdue ? Colors.Red.Darken2 : Colors.Grey.Darken3)
                                            .FontSize(10);
                                        table.Cell().Text(isOverdue ? "Overdue" : "Active")
                                            .FontColor(isOverdue ? Colors.Red.Darken2 : Colors.Green.Darken2)
                                            .AlignRight()
                                            .FontSize(10);
                                    }
                                });
                        }
                        
                        // Expenses Section
                        if (Program.expenses?.Count > 0)
                        {
                            column.Item()
                                .PaddingBottom(10)
                                .Text("EXPENSE TRACKING")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Blue.Darken3);
                            
                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(30);  // №
                                        columns.RelativeColumn();    // Date
                                        columns.RelativeColumn(2);   // Description
                                        columns.RelativeColumn();    // Category
                                        columns.RelativeColumn();    // Amount
                                    });
                                    
                                    // Table Header
                                    table.Header(header =>
                                    {
                                        header.Cell().Text("#").Bold();
                                        header.Cell().Text("Date").Bold();
                                        header.Cell().Text("Description").Bold();
                                        header.Cell().Text("Category").Bold();
                                        header.Cell().Text("Amount").Bold().AlignRight();
                                    });
                                    
                                    // Expenses Data
                                    for (int i = 0; i < Program.expenses.Count; i++)
                                    {
                                        var expense = Program.expenses[i];
                                        table.Cell().Text($"{i + 1}").FontSize(10);
                                        table.Cell().Text(expense.DateDisplay).FontSize(10);
                                        table.Cell().Text(expense.Description).FontSize(11);
                                        table.Cell().Text(expense.Category.ToString()).FontSize(10);
                                        table.Cell().Text($"{expense.Amount:C}").AlignRight().FontSize(11);
                                    }
                                    
                                    // Total Row
                                    if (Program.expenses.Count > 0)
                                    {
                                        table.Cell().ColumnSpan(4).Text("TOTAL").Bold().AlignRight();
                                        table.Cell().Text($"{totalExpenses:C}").Bold().AlignRight().FontColor(Colors.Red.Darken2);
                                    }
                                });
                        }
                        
                        // Footer Note
                        column.Item()
                            .PaddingTop(40)
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .PaddingTop(10)
                            .Text("Generated by Finance Flow Planner | Confidential Financial Document")
                            .FontSize(8)
                            .FontColor(Colors.Grey.Medium)
                            .AlignCenter();
                    });
                
                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
            });
        })
        .GeneratePdf(fileName);

        Console.Clear();
        ConsoleRenderer.DrawHeader("    📤 PDF EXPORT COMPLETED      ", ConsoleColor.DarkCyan);
        Console.WriteLine($"║ 📄 File: {fileName}");
        Console.WriteLine($"║ 📊 Goals: {Program.goals?.Count ?? 0} items");
        Console.WriteLine($"║ 💰 Expenses: {Program.expenses?.Count ?? 0} items");
        Console.WriteLine("╚═══════════════════════════════════════════");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
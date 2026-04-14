using DeviceTracker.Models;

namespace DeviceTracker.UI;

public static class ConsoleHelper
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', title.Length + 4) + "╗");
        Console.WriteLine($"║  {title}  ║");
        Console.WriteLine("╚" + new string('═', title.Length + 4) + "╝");
        Console.ResetColor();
    }

    public static void PrintSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {msg}");
        Console.ResetColor();
    }

    public static void PrintError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {msg}");
        Console.ResetColor();
    }

    public static void PrintWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠ {msg}");
        Console.ResetColor();
    }

    public static void PrintInfo(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  {msg}");
        Console.ResetColor();
    }

    public static void PrintDeviceTable(IEnumerable<Device> devices)
    {
        var list = devices.ToList();
        if (!list.Any())
        {
            PrintWarning("No devices found.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n  {"ID",-10} {"Name",-28} {"Type",-18} {"Status",-16} {"System",-20} {"Assigned To",-18}");
        Console.WriteLine("  " + new string('─', 112));
        Console.ResetColor();

        foreach (var d in list)
        {
            Console.ForegroundColor = StatusColor(d.Status);
            Console.WriteLine($"  {d.Id,-10} {d.Name,-28} {d.Type,-18} {d.Status,-16} {d.SystemName,-20} {d.AssignedTo ?? "-",-18}");
            Console.ResetColor();
        }
        Console.WriteLine($"\n  Total: {list.Count} device(s)");
    }

    public static void PrintIssueTable(IEnumerable<Issue> issues)
    {
        var list = issues.ToList();
        if (!list.Any())
        {
            PrintWarning("No issues found.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n  {"ID",-10} {"Sev",-10} {"Status",-12} {"Reported By",-18} {"Description",-40}");
        Console.WriteLine("  " + new string('─', 94));
        Console.ResetColor();

        foreach (var i in list)
        {
            Console.ForegroundColor = SeverityColor(i.Severity);
            var desc = i.Description.Length > 38 ? i.Description[..35] + "..." : i.Description;
            Console.WriteLine($"  {i.Id,-10} {i.Severity,-10} {i.Status,-12} {i.ReportedBy,-18} {desc,-40}");
            Console.ResetColor();
        }
        Console.WriteLine($"\n  Total: {list.Count} issue(s)");
    }

    public static string Prompt(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"  {message}: ");
        Console.ResetColor();
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static string PromptOptional(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"  {message} (optional, press Enter to skip): ");
        Console.ResetColor();
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static T PromptEnum<T>(string message) where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        Console.WriteLine($"\n  {message}:");
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine($"    {i + 1}. {values[i]}");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Choose (number): ");
            Console.ResetColor();
            if (int.TryParse(Console.ReadLine(), out int choice) &&
                choice >= 1 && choice <= values.Length)
                return values[choice - 1];
            PrintError("Invalid choice. Try again.");
        }
    }

    public static bool Confirm(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"  {message} (y/n): ");
        Console.ResetColor();
        return Console.ReadLine()?.Trim().ToLower() == "y";
    }

    public static void PressEnterToContinue()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("\n  Press Enter to continue...");
        Console.ResetColor();
        Console.ReadLine();
    }

    private static ConsoleColor StatusColor(DeviceStatus s) => s switch
    {
        DeviceStatus.Active => ConsoleColor.Green,
        DeviceStatus.Faulty => ConsoleColor.Red,
        DeviceStatus.Installed => ConsoleColor.Cyan,
        DeviceStatus.Configured => ConsoleColor.Blue,
        DeviceStatus.Pending => ConsoleColor.Yellow,
        DeviceStatus.Decommissioned => ConsoleColor.DarkGray,
        _ => ConsoleColor.White
    };

    private static ConsoleColor SeverityColor(IssueSeverity s) => s switch
    {
        IssueSeverity.Critical => ConsoleColor.Red,
        IssueSeverity.High => ConsoleColor.DarkRed,
        IssueSeverity.Medium => ConsoleColor.Yellow,
        IssueSeverity.Low => ConsoleColor.DarkGray,
        _ => ConsoleColor.White
    };
}

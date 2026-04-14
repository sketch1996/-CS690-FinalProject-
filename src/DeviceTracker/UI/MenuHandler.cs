using DeviceTracker.Models;
using DeviceTracker.Services;

namespace DeviceTracker.UI;

public class MenuHandler
{
    private readonly DeviceService _deviceService;
    private readonly IssueService _issueService;
    private readonly AssignmentService _assignmentService;
    private readonly ReportService _reportService;

    public MenuHandler(DeviceService deviceService, IssueService issueService,
        AssignmentService assignmentService, ReportService reportService)
    {
        _deviceService = deviceService;
        _issueService = issueService;
        _assignmentService = assignmentService;
        _reportService = reportService;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("Device Tracker – Data Center Management");
            Console.WriteLine();
            Console.WriteLine("  1.  Manage Devices");
            Console.WriteLine("  2.  Manage Issues");
            Console.WriteLine("  3.  Manage Assignments");
            Console.WriteLine("  4.  Reports");
            Console.WriteLine("  0.  Exit");
            Console.WriteLine();

            var choice = ConsoleHelper.Prompt("Select option");
            switch (choice)
            {
                case "1": DeviceMenu(); break;
                case "2": IssueMenu(); break;
                case "3": AssignmentMenu(); break;
                case "4": ReportMenu(); break;
                case "0": return;
                default: ConsoleHelper.PrintError("Invalid option."); Thread.Sleep(800); break;
            }
        }
    }

    // ─── DEVICE MENU ────────────────────────────────────────────────────────
    private void DeviceMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("Manage Devices");
            Console.WriteLine();
            Console.WriteLine("  1.  List all devices");
            Console.WriteLine("  2.  Add new device");
            Console.WriteLine("  3.  View device details");
            Console.WriteLine("  4.  Update device status");
            Console.WriteLine("  5.  Assign device to technician");
            Console.WriteLine("  6.  Filter / search devices");
            Console.WriteLine("  7.  Delete device");
            Console.WriteLine("  0.  Back");
            Console.WriteLine();

            var choice = ConsoleHelper.Prompt("Select option");
            switch (choice)
            {
                case "1": ListAllDevices(); break;
                case "2": AddDevice(); break;
                case "3": ViewDeviceDetails(); break;
                case "4": UpdateDeviceStatus(); break;
                case "5": AssignDevice(); break;
                case "6": FilterDevices(); break;
                case "7": DeleteDevice(); break;
                case "0": return;
                default: ConsoleHelper.PrintError("Invalid option."); Thread.Sleep(800); break;
            }
        }
    }

    private void ListAllDevices()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("All Devices");
        ConsoleHelper.PrintDeviceTable(_deviceService.GetAll());
        ConsoleHelper.PressEnterToContinue();
    }

    private void AddDevice()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Add New Device");
        var name = ConsoleHelper.Prompt("Device name");
        if (string.IsNullOrWhiteSpace(name)) return;
        var type = ConsoleHelper.PromptEnum<DeviceType>("Device type");
        var system = ConsoleHelper.Prompt("System name (e.g. HVAC-1, Network-A)");
        var location = ConsoleHelper.Prompt("Location (e.g. Rack A1-U12)");
        var ip = ConsoleHelper.PromptOptional("IP address");

        var device = _deviceService.AddDevice(name, type, system, location,
            string.IsNullOrWhiteSpace(ip) ? null : ip);
        ConsoleHelper.PrintSuccess($"Device added: {device.Id} – {device.Name}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void ViewDeviceDetails()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Device Details");
        var id = ConsoleHelper.Prompt("Enter device ID");
        var device = _deviceService.GetById(id);
        if (device == null) { ConsoleHelper.PrintError("Device not found."); ConsoleHelper.PressEnterToContinue(); return; }

        Console.WriteLine();
        Console.WriteLine($"  ID          : {device.Id}");
        Console.WriteLine($"  Name        : {device.Name}");
        Console.WriteLine($"  Type        : {device.Type}");
        Console.WriteLine($"  Status      : {device.Status}");
        Console.WriteLine($"  System      : {device.SystemName}");
        Console.WriteLine($"  Location    : {device.Location}");
        Console.WriteLine($"  IP Address  : {device.IpAddress ?? "Not set"}");
        Console.WriteLine($"  Assigned To : {device.AssignedTo ?? "Unassigned"}");
        Console.WriteLine($"  Created     : {device.CreatedAt:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Updated     : {device.UpdatedAt:yyyy-MM-dd HH:mm}");

        var issues = _issueService.GetByDevice(device.Id).ToList();
        Console.WriteLine($"\n  Issues ({issues.Count}):");
        ConsoleHelper.PrintIssueTable(issues);

        var assignments = _assignmentService.GetByDevice(device.Id).ToList();
        Console.WriteLine($"\n  Assignments ({assignments.Count}):");
        foreach (var a in assignments)
            Console.WriteLine($"    {a}");

        ConsoleHelper.PressEnterToContinue();
    }

    private void UpdateDeviceStatus()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Update Device Status");
        var id = ConsoleHelper.Prompt("Enter device ID");
        var device = _deviceService.GetById(id);
        if (device == null) { ConsoleHelper.PrintError("Device not found."); ConsoleHelper.PressEnterToContinue(); return; }
        Console.WriteLine($"\n  Current status: {device.Status}");
        var newStatus = ConsoleHelper.PromptEnum<DeviceStatus>("New status");
        _deviceService.UpdateStatus(id, newStatus);
        ConsoleHelper.PrintSuccess($"Status updated to {newStatus}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void AssignDevice()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Assign Device to Technician");
        var id = ConsoleHelper.Prompt("Enter device ID");
        var device = _deviceService.GetById(id);
        if (device == null) { ConsoleHelper.PrintError("Device not found."); ConsoleHelper.PressEnterToContinue(); return; }
        Console.WriteLine($"\n  Device: {device.Name}  (currently: {device.AssignedTo ?? "unassigned"})");
        var name = ConsoleHelper.Prompt("Technician name");
        var task = ConsoleHelper.Prompt("Task description");
        _deviceService.AssignDevice(id, name);
        _assignmentService.CreateAssignment(id, name, task);
        ConsoleHelper.PrintSuccess($"Device assigned to {name}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void FilterDevices()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Filter / Search Devices");
        Console.WriteLine("  (Press Enter to skip any filter)");
        Console.WriteLine();

        DeviceStatus? status = null;
        DeviceType? type = null;

        if (ConsoleHelper.Confirm("Filter by status?"))
            status = ConsoleHelper.PromptEnum<DeviceStatus>("Select status");
        if (ConsoleHelper.Confirm("Filter by type?"))
            type = ConsoleHelper.PromptEnum<DeviceType>("Select type");
        var system = ConsoleHelper.PromptOptional("System name contains");
        var assigned = ConsoleHelper.PromptOptional("Assigned to (name contains)");

        var results = _deviceService.Filter(status, type,
            string.IsNullOrWhiteSpace(system) ? null : system,
            string.IsNullOrWhiteSpace(assigned) ? null : assigned);
        ConsoleHelper.PrintDeviceTable(results);
        ConsoleHelper.PressEnterToContinue();
    }

    private void DeleteDevice()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Delete Device");
        var id = ConsoleHelper.Prompt("Enter device ID");
        var device = _deviceService.GetById(id);
        if (device == null) { ConsoleHelper.PrintError("Device not found."); ConsoleHelper.PressEnterToContinue(); return; }
        Console.WriteLine($"\n  Device: {device}");
        if (ConsoleHelper.Confirm("Are you sure you want to delete this device?"))
        {
            _deviceService.DeleteDevice(id);
            ConsoleHelper.PrintSuccess("Device deleted.");
        }
        ConsoleHelper.PressEnterToContinue();
    }

    // ─── ISSUE MENU ─────────────────────────────────────────────────────────
    private void IssueMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("Manage Issues");
            Console.WriteLine();
            Console.WriteLine("  1.  List all open issues");
            Console.WriteLine("  2.  List all issues");
            Console.WriteLine("  3.  Log new issue");
            Console.WriteLine("  4.  Resolve issue");
            Console.WriteLine("  5.  Update issue status");
            Console.WriteLine("  0.  Back");
            Console.WriteLine();

            var choice = ConsoleHelper.Prompt("Select option");
            switch (choice)
            {
                case "1": ListOpenIssues(); break;
                case "2": ListAllIssues(); break;
                case "3": LogIssue(); break;
                case "4": ResolveIssue(); break;
                case "5": UpdateIssueStatus(); break;
                case "0": return;
                default: ConsoleHelper.PrintError("Invalid option."); Thread.Sleep(800); break;
            }
        }
    }

    private void ListOpenIssues()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Open Issues");
        ConsoleHelper.PrintIssueTable(_issueService.GetOpen());
        ConsoleHelper.PressEnterToContinue();
    }

    private void ListAllIssues()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("All Issues");
        ConsoleHelper.PrintIssueTable(_issueService.GetAll());
        ConsoleHelper.PressEnterToContinue();
    }

    private void LogIssue()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Log New Issue");
        var deviceId = ConsoleHelper.Prompt("Device ID");
        var device = _deviceService.GetById(deviceId);
        if (device == null) { ConsoleHelper.PrintError("Device not found."); ConsoleHelper.PressEnterToContinue(); return; }
        var desc = ConsoleHelper.Prompt("Issue description");
        var severity = ConsoleHelper.PromptEnum<IssueSeverity>("Severity");
        var reporter = ConsoleHelper.Prompt("Reported by");
        var issue = _issueService.LogIssue(deviceId, desc, severity, reporter);
        ConsoleHelper.PrintSuccess($"Issue logged: {issue.Id}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void ResolveIssue()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Resolve Issue");
        var issueId = ConsoleHelper.Prompt("Issue ID");
        var resolvedBy = ConsoleHelper.Prompt("Resolved by");
        var notes = ConsoleHelper.Prompt("Resolution notes");
        if (_issueService.ResolveIssue(issueId, resolvedBy, notes))
            ConsoleHelper.PrintSuccess("Issue resolved.");
        else
            ConsoleHelper.PrintError("Issue not found.");
        ConsoleHelper.PressEnterToContinue();
    }

    private void UpdateIssueStatus()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Update Issue Status");
        var issueId = ConsoleHelper.Prompt("Issue ID");
        var status = ConsoleHelper.PromptEnum<IssueStatus>("New status");
        if (_issueService.UpdateStatus(issueId, status))
            ConsoleHelper.PrintSuccess($"Status updated to {status}");
        else
            ConsoleHelper.PrintError("Issue not found.");
        ConsoleHelper.PressEnterToContinue();
    }

    // ─── ASSIGNMENT MENU ─────────────────────────────────────────────────────
    private void AssignmentMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("Manage Assignments");
            Console.WriteLine();
            Console.WriteLine("  1.  List all assignments");
            Console.WriteLine("  2.  View by technician");
            Console.WriteLine("  3.  Complete assignment");
            Console.WriteLine("  4.  Team workload summary");
            Console.WriteLine("  0.  Back");
            Console.WriteLine();

            var choice = ConsoleHelper.Prompt("Select option");
            switch (choice)
            {
                case "1": ListAllAssignments(); break;
                case "2": ViewByTechnician(); break;
                case "3": CompleteAssignment(); break;
                case "4": WorkloadSummary(); break;
                case "0": return;
                default: ConsoleHelper.PrintError("Invalid option."); Thread.Sleep(800); break;
            }
        }
    }

    private void ListAllAssignments()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("All Assignments");
        var all = _assignmentService.GetAll().ToList();
        if (!all.Any()) { ConsoleHelper.PrintWarning("No assignments found."); ConsoleHelper.PressEnterToContinue(); return; }
        Console.WriteLine($"\n  {"ID",-10} {"Technician",-20} {"Task",-35} {"Status",-12} {"Device",-10}");
        Console.WriteLine("  " + new string('─', 92));
        foreach (var a in all)
            Console.WriteLine($"  {a.Id,-10} {a.TechnicianName,-20} {a.Task[..Math.Min(33, a.Task.Length)],- 35} {(a.IsCompleted ? "Completed" : "Pending"),-12} {a.DeviceId,-10}");
        Console.WriteLine($"\n  Total: {all.Count}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void ViewByTechnician()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Assignments by Technician");
        var name = ConsoleHelper.Prompt("Technician name");
        var results = _assignmentService.GetByTechnician(name).ToList();
        if (!results.Any()) { ConsoleHelper.PrintWarning("No assignments found."); ConsoleHelper.PressEnterToContinue(); return; }
        foreach (var a in results)
            Console.WriteLine($"  {a}  Device: {a.DeviceId}  {(a.Notes != null ? "Notes: " + a.Notes : "")}");
        ConsoleHelper.PressEnterToContinue();
    }

    private void CompleteAssignment()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Complete Assignment");
        var id = ConsoleHelper.Prompt("Assignment ID");
        var notes = ConsoleHelper.Prompt("Completion notes");
        if (_assignmentService.CompleteAssignment(id, notes))
            ConsoleHelper.PrintSuccess("Assignment marked complete.");
        else
            ConsoleHelper.PrintError("Assignment not found.");
        ConsoleHelper.PressEnterToContinue();
    }

    private void WorkloadSummary()
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Team Workload Summary");
        var workload = _assignmentService.GetTechnicianWorkload();
        if (!workload.Any()) { ConsoleHelper.PrintWarning("No active assignments."); ConsoleHelper.PressEnterToContinue(); return; }
        Console.WriteLine();
        foreach (var kv in workload.OrderByDescending(k => k.Value))
            Console.WriteLine($"  {kv.Key,-25} {kv.Value} open task(s)");
        ConsoleHelper.PressEnterToContinue();
    }

    // ─── REPORT MENU ─────────────────────────────────────────────────────────
    private void ReportMenu()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("Reports");
            Console.WriteLine();
            Console.WriteLine("  1.  Full status report");
            Console.WriteLine("  2.  System-specific report");
            Console.WriteLine("  3.  Export status report to file");
            Console.WriteLine("  0.  Back");
            Console.WriteLine();

            var choice = ConsoleHelper.Prompt("Select option");
            switch (choice)
            {
                case "1": ShowStatusReport(); break;
                case "2": ShowSystemReport(); break;
                case "3": ExportReport(); break;
                case "0": return;
                default: ConsoleHelper.PrintError("Invalid option."); Thread.Sleep(800); break;
            }
        }
    }

    private void ShowStatusReport()
    {
        Console.Clear();
        Console.WriteLine(_reportService.GenerateStatusReport());
        ConsoleHelper.PressEnterToContinue();
    }

    private void ShowSystemReport()
    {
        Console.Clear();
        var system = ConsoleHelper.Prompt("System name");
        Console.WriteLine(_reportService.GenerateSystemReport(system));
        ConsoleHelper.PressEnterToContinue();
    }

    private void ExportReport()
    {
        var report = _reportService.GenerateStatusReport();
        var path = _reportService.ExportReportToFile(report, "StatusReport");
        ConsoleHelper.PrintSuccess($"Report saved to: {path}");
        ConsoleHelper.PressEnterToContinue();
    }
}

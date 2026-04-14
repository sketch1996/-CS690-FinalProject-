using DeviceTracker.Data;
using DeviceTracker.Models;

namespace DeviceTracker.Services;

public class ReportService
{
    private readonly DataStore _store;

    public ReportService(DataStore store)
    {
        _store = store;
    }

    public string GenerateStatusReport()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== DEVICE STATUS REPORT ===");
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine($"Total Devices: {_store.Devices.Count}");
        sb.AppendLine();

        var byStatus = _store.Devices.GroupBy(d => d.Status);
        sb.AppendLine("--- By Status ---");
        foreach (var g in byStatus.OrderBy(g => g.Key.ToString()))
            sb.AppendLine($"  {g.Key,-20} {g.Count(),4} device(s)");

        sb.AppendLine();
        sb.AppendLine("--- By System ---");
        var bySystem = _store.Devices.GroupBy(d => d.SystemName);
        foreach (var g in bySystem.OrderBy(g => g.Key))
            sb.AppendLine($"  {g.Key,-25} {g.Count(),4} device(s)");

        sb.AppendLine();
        var openIssues = _store.Issues.Where(i => i.Status != IssueStatus.Resolved).ToList();
        sb.AppendLine($"--- Open Issues: {openIssues.Count} ---");
        foreach (var i in openIssues.OrderByDescending(i => i.Severity))
            sb.AppendLine($"  [{i.Severity}] {i.Description[..Math.Min(60, i.Description.Length)]}");

        return sb.ToString();
    }

    public string GenerateSystemReport(string systemName)
    {
        var devices = _store.Devices
            .Where(d => d.SystemName.Equals(systemName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"=== SYSTEM REPORT: {systemName.ToUpper()} ===");
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine($"Total Devices: {devices.Count}");
        sb.AppendLine();

        foreach (var d in devices.OrderBy(d => d.Name))
        {
            sb.AppendLine($"  {d.Id}  {d.Name,-30} {d.Type,-20} {d.Status}");
            if (d.AssignedTo != null)
                sb.AppendLine($"         Assigned: {d.AssignedTo}");
            if (d.IpAddress != null)
                sb.AppendLine($"         IP: {d.IpAddress}");

            var issues = _store.Issues
                .Where(i => i.DeviceId == d.Id && i.Status != IssueStatus.Resolved)
                .ToList();
            if (issues.Any())
                sb.AppendLine($"         Open Issues: {issues.Count}");
        }
        return sb.ToString();
    }

    public string ExportReportToFile(string report, string fileName)
    {
        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "DeviceTrackerReports");
        Directory.CreateDirectory(path);
        var fullPath = Path.Combine(path, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmm}.txt");
        File.WriteAllText(fullPath, report);
        return fullPath;
    }
}

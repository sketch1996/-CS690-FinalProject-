namespace DeviceTracker.Models;

public enum IssueSeverity { Low, Medium, High, Critical }
public enum IssueStatus { Open, InProgress, Resolved }

public class Issue
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8].ToUpper();
    public string DeviceId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IssueSeverity Severity { get; set; } = IssueSeverity.Medium;
    public IssueStatus Status { get; set; } = IssueStatus.Open;
    public string ReportedBy { get; set; } = string.Empty;
    public string? ResolvedBy { get; set; }
    public DateTime ReportedAt { get; set; } = DateTime.Now;
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }

    public override string ToString() =>
        $"[{Id}] {Severity} | {Status} | {Description[..Math.Min(50, Description.Length)]}";
}

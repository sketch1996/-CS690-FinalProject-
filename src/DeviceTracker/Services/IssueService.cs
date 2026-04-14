using DeviceTracker.Data;
using DeviceTracker.Models;

namespace DeviceTracker.Services;

public class IssueService
{
    private readonly DataStore _store;

    public IssueService(DataStore store)
    {
        _store = store;
    }

    public Issue LogIssue(string deviceId, string description, IssueSeverity severity, string reportedBy)
    {
        var issue = new Issue
        {
            DeviceId = deviceId,
            Description = description,
            Severity = severity,
            ReportedBy = reportedBy
        };
        _store.Issues.Add(issue);

        var device = _store.Devices.FirstOrDefault(d => d.Id == deviceId);
        device?.IssueIds.Add(issue.Id);

        _store.Save();
        return issue;
    }

    public IEnumerable<Issue> GetByDevice(string deviceId) =>
        _store.Issues.Where(i => i.DeviceId == deviceId);

    public IEnumerable<Issue> GetAll() => _store.Issues;

    public IEnumerable<Issue> GetOpen() =>
        _store.Issues.Where(i => i.Status != IssueStatus.Resolved);

    public bool ResolveIssue(string issueId, string resolvedBy, string notes)
    {
        var issue = _store.Issues.FirstOrDefault(i =>
            i.Id.Equals(issueId, StringComparison.OrdinalIgnoreCase));
        if (issue == null) return false;
        issue.Status = IssueStatus.Resolved;
        issue.ResolvedBy = resolvedBy;
        issue.ResolutionNotes = notes;
        issue.ResolvedAt = DateTime.Now;
        _store.Save();
        return true;
    }

    public bool UpdateStatus(string issueId, IssueStatus status)
    {
        var issue = _store.Issues.FirstOrDefault(i =>
            i.Id.Equals(issueId, StringComparison.OrdinalIgnoreCase));
        if (issue == null) return false;
        issue.Status = status;
        _store.Save();
        return true;
    }
}

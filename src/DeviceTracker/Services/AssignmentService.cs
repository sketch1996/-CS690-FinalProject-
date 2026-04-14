using DeviceTracker.Data;
using DeviceTracker.Models;

namespace DeviceTracker.Services;

public class AssignmentService
{
    private readonly DataStore _store;

    public AssignmentService(DataStore store)
    {
        _store = store;
    }

    public Assignment CreateAssignment(string deviceId, string technicianName, string task)
    {
        var assignment = new Assignment
        {
            DeviceId = deviceId,
            TechnicianName = technicianName,
            Task = task
        };
        _store.Assignments.Add(assignment);
        _store.Save();
        return assignment;
    }

    public IEnumerable<Assignment> GetByDevice(string deviceId) =>
        _store.Assignments.Where(a => a.DeviceId == deviceId);

    public IEnumerable<Assignment> GetByTechnician(string name) =>
        _store.Assignments.Where(a =>
            a.TechnicianName.Contains(name, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Assignment> GetAll() => _store.Assignments;

    public bool CompleteAssignment(string assignmentId, string notes)
    {
        var a = _store.Assignments.FirstOrDefault(a =>
            a.Id.Equals(assignmentId, StringComparison.OrdinalIgnoreCase));
        if (a == null) return false;
        a.CompletedAt = DateTime.Now;
        a.Notes = notes;
        _store.Save();
        return true;
    }

    public Dictionary<string, int> GetTechnicianWorkload() =>
        _store.Assignments
            .Where(a => !a.IsCompleted)
            .GroupBy(a => a.TechnicianName)
            .ToDictionary(g => g.Key, g => g.Count());
}

namespace DeviceTracker.Models;

public class Assignment
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8].ToUpper();
    public string DeviceId { get; set; } = string.Empty;
    public string TechnicianName { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted => CompletedAt.HasValue;

    public override string ToString() =>
        $"[{Id}] {TechnicianName} → {Task} | {(IsCompleted ? "Done" : "Pending")}";
}

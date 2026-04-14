namespace DeviceTracker.Models;

public enum DeviceStatus
{
    Pending,
    Installed,
    Configured,
    Active,
    Faulty,
    Decommissioned
}

public enum DeviceType
{
    Controller,
    Sensor,
    NetworkEquipment,
    Server,
    StorageUnit,
    PowerUnit,
    Other
}

public class Device
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..8].ToUpper();
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public DeviceStatus Status { get; set; } = DeviceStatus.Pending;
    public string SystemName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<string> IssueIds { get; set; } = new();

    public override string ToString() =>
        $"[{Id}] {Name} | {Type} | {Status} | System: {SystemName} | Location: {Location}";
}

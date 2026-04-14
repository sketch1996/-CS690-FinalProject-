using DeviceTracker.Data;
using DeviceTracker.Models;

namespace DeviceTracker.Services;

public class DeviceService
{
    private readonly DataStore _store;

    public DeviceService(DataStore store)
    {
        _store = store;
    }

    public Device AddDevice(string name, DeviceType type, string systemName, string location, string? ip = null)
    {
        var device = new Device
        {
            Name = name,
            Type = type,
            SystemName = systemName,
            Location = location,
            IpAddress = ip
        };
        _store.Devices.Add(device);
        _store.Save();
        return device;
    }

    public Device? GetById(string id) =>
        _store.Devices.FirstOrDefault(d => d.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Device> GetAll() => _store.Devices;

    public IEnumerable<Device> Filter(
        DeviceStatus? status = null,
        DeviceType? type = null,
        string? systemName = null,
        string? assignedTo = null)
    {
        var q = _store.Devices.AsEnumerable();
        if (status.HasValue) q = q.Where(d => d.Status == status);
        if (type.HasValue) q = q.Where(d => d.Type == type);
        if (!string.IsNullOrWhiteSpace(systemName))
            q = q.Where(d => d.SystemName.Contains(systemName, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(assignedTo))
            q = q.Where(d => d.AssignedTo != null &&
                d.AssignedTo.Contains(assignedTo, StringComparison.OrdinalIgnoreCase));
        return q;
    }

    public bool UpdateStatus(string id, DeviceStatus newStatus)
    {
        var device = GetById(id);
        if (device == null) return false;
        device.Status = newStatus;
        device.UpdatedAt = DateTime.Now;
        _store.Save();
        return true;
    }

    public bool AssignDevice(string id, string technicianName)
    {
        var device = GetById(id);
        if (device == null) return false;
        device.AssignedTo = technicianName;
        device.UpdatedAt = DateTime.Now;
        _store.Save();
        return true;
    }

    public bool UpdateDevice(Device device)
    {
        var existing = GetById(device.Id);
        if (existing == null) return false;
        var idx = _store.Devices.IndexOf(existing);
        device.UpdatedAt = DateTime.Now;
        _store.Devices[idx] = device;
        _store.Save();
        return true;
    }

    public bool DeleteDevice(string id)
    {
        var device = GetById(id);
        if (device == null) return false;
        _store.Devices.Remove(device);
        _store.Save();
        return true;
    }

    public Dictionary<DeviceStatus, int> GetStatusSummary() =>
        _store.Devices
            .GroupBy(d => d.Status)
            .ToDictionary(g => g.Key, g => g.Count());

    public Dictionary<string, int> GetSystemSummary() =>
        _store.Devices
            .GroupBy(d => d.SystemName)
            .ToDictionary(g => g.Key, g => g.Count());
}

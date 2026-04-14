using System.Text.Json;
using DeviceTracker.Models;

namespace DeviceTracker.Data;

public class DataStore
{
    private readonly string _dataDirectory;
    private readonly string _devicesFile;
    private readonly string _issuesFile;
    private readonly string _assignmentsFile;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public List<Device> Devices { get; private set; } = new();
    public List<Issue> Issues { get; private set; } = new();
    public List<Assignment> Assignments { get; private set; } = new();

    public DataStore(string? dataDirectory = null)
    {
        _dataDirectory = dataDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DeviceTracker");
        _devicesFile = Path.Combine(_dataDirectory, "devices.json");
        _issuesFile = Path.Combine(_dataDirectory, "issues.json");
        _assignmentsFile = Path.Combine(_dataDirectory, "assignments.json");
        Directory.CreateDirectory(_dataDirectory);
        Load();
    }

    public void Load()
    {
        Devices = LoadFile<List<Device>>(_devicesFile) ?? new();
        Issues = LoadFile<List<Issue>>(_issuesFile) ?? new();
        Assignments = LoadFile<List<Assignment>>(_assignmentsFile) ?? new();
    }

    public void Save()
    {
        SaveFile(_devicesFile, Devices);
        SaveFile(_issuesFile, Issues);
        SaveFile(_assignmentsFile, Assignments);
    }

    private T? LoadFile<T>(string path)
    {
        if (!File.Exists(path)) return default;
        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch { return default; }
    }

    private void SaveFile<T>(string path, T data)
    {
        var json = JsonSerializer.Serialize(data, JsonOptions);
        File.WriteAllText(path, json);
    }
}

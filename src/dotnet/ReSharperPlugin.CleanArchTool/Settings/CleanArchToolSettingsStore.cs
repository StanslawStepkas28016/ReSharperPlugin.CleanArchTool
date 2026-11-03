using System;
using System.IO;
using JetBrains.Application.Parts;
using JetBrains.ProjectModel;
using JetBrains.Util;
using Newtonsoft.Json;

namespace ReSharperPlugin.CleanArchTool.Settings;

[SolutionComponent(Instantiation.DemandAnyThreadSafe)]
public class CleanArchToolSettingsStore
{
    public const string ConfigurationFileName = ".cleanarchtool.json";

    private static readonly ILogger Logger = JetBrains.Util.Logging.Logger.GetLogger<CleanArchToolSettingsStore>();

    private readonly object _lock = new();

    private CleanArchToolSettings _cachedSettings;
    private DateTime _cachedWriteTime;
    private readonly ISolution _solution;

    public CleanArchToolSettingsStore(
        ISolution solution
    )
    {
        _solution = solution;
    }

    private string ConfigurationFilePath =>
        Path.Combine(_solution.SolutionDirectory.FullPath, ConfigurationFileName);

    public bool ConfigurationFileExists()
    {
        return File.Exists(ConfigurationFilePath);
    }

    public CleanArchToolSettings GetSettings()
    {
        lock (_lock)
        {
            var path = ConfigurationFilePath;
            if (!File.Exists(path)) return new CleanArchToolSettings();

            var writeTime = File.GetLastWriteTimeUtc(path);
            if (_cachedSettings == null || writeTime != _cachedWriteTime)
            {
                _cachedSettings = ReadSettings(path);
                _cachedWriteTime = writeTime;
            }

            return _cachedSettings.Clone();
        }
    }

    public void SetSettings(
        CleanArchToolSettings settings
    )
    {
        lock (_lock)
        {
            File.WriteAllText(ConfigurationFilePath, JsonConvert.SerializeObject(settings, Formatting.Indented));
            _cachedSettings = null;
        }
    }

    public bool CreateConfigurationFile()
    {
        lock (_lock)
        {
            if (ConfigurationFileExists()) return false;

            SetSettings(new CleanArchToolSettings());
            return true;
        }
    }

    private CleanArchToolSettings ReadSettings(
        string path
    )
    {
        try
        {
            return JsonConvert.DeserializeObject<CleanArchToolSettings>(File.ReadAllText(path))
                   ?? new CleanArchToolSettings();
        }
        catch (Exception e) when (e is JsonException or IOException)
        {
            Logger.Warn($"Failed to read {path}, falling back to default settings: {e.Message}");
            return new CleanArchToolSettings();
        }
    }
}
namespace ReSharperPlugin.CleanArchTool.Settings;

/// <summary>
///     Per-solution plugin settings, serialized to <see cref="CleanArchToolSettingsStore.ConfigurationFileName" />
///     in the solution root directory.
/// </summary>
public class CleanArchToolSettings
{
    public int MaxClassLines { get; set; } = 100;

    public bool MaxClassLinesEnabled { get; set; } = true;

    public int MaxClassMethods { get; set; } = 5;

    public bool MaxClassMethodsEnabled { get; set; } = true;

    public int MaxCtorArguments { get; set; } = 5;

    public bool MaxCtorArgumentsEnabled { get; set; } = true;

    public int MaxMethodArguments { get; set; } = 5;

    public bool MaxMethodArgumentsEnabled { get; set; } = true;

    public bool CheckMultipleMethods { get; set; } = true;

    public string PresentationLayerProjectFileName { get; set; } = "";

    public string ApplicationLayerProjectFileName { get; set; } = "";

    public string InfrastructureLayerProjectFileName { get; set; } = "";

    public string DomainLayerProjectFileName { get; set; } = "";

    public string SharedLibrariesPackageNames { get; set; }

    public CleanArchToolSettings Clone()
    {
        return (CleanArchToolSettings)MemberwiseClone();
    }
}
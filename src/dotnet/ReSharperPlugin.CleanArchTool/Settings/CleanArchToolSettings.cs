namespace ReSharperPlugin.CleanArchTool.Settings;

/// <summary>
///     Per-solution plugin settings, serialized to <see cref="CleanArchToolSettingsStore.ConfigurationFileName" />
///     in the solution root directory.
/// </summary>
public class CleanArchToolSettings
{
    public int MaxClassLines { get; set; } = 100;
    
    public int MaxClassMethods { get; set; } = 5;

    public string PresentationLayerProjectFileName { get; set; } = "";

    public string ApplicationLayerProjectFileName { get; set; } = "";

    public string InfrastructureLayerProjectFileName { get; set; } = "";

    public string DomainLayerProjectFileName { get; set; } = "";

    public string SharedLibrariesPackageNames { get; set; }

    public CleanArchToolSettings Clone() => (CleanArchToolSettings)MemberwiseClone();
}

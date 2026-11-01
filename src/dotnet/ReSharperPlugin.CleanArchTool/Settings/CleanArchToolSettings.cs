using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace ReSharperPlugin.CleanArchTool.Settings;

[SettingsKey(typeof(CodeInspectionSettings), "CleanArchToolSettings")]
public class CleanArchToolSettings
{
    [SettingsEntry(15, "Max lines in a class")]
    public int MaxClassLines;

    [SettingsEntry("", "Presentation layer project file name")]
    public string PresentationLayerProjectFileName;

    [SettingsEntry("", "Application layer project file name")]
    public string ApplicationLayerProjectFileName;

    [SettingsEntry("", "Infrastructure layer project file name")]
    public string InfrastructureLayerProjectFileName;

    [SettingsEntry("", "Domain layer project file name")]
    public string DomainLayerProjectFileName;

    [SettingsEntry(null, "Shared libraries package names")]
    public string SharedLibrariesPackageNames;
}
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;
using JetBrains.Util;

namespace ReSharperPlugin.CleanArchTool;

[SettingsKey(typeof(CodeInspectionSettings), "CleanArchTool - Settings")]
public class CleanArchSettings
{
    [SettingsEntry(DefaultValue: 10, Description: "Max lines in a class")]
    public int MaxClassLines;

    [SettingsEntry(DefaultValue: "", Description: "Presentation layer project file")]
    public FileSystemPath PresentationLayerProjectFile;
    
    [SettingsEntry(DefaultValue: "", Description: "Application layer project file")]
    public FileSystemPath ApplicationLayerProjectFile;
    
    [SettingsEntry(DefaultValue: "", Description: "Infrastructure layer project file")]
    public FileSystemPath InfrastructureLayerProjectFile;

    [SettingsEntry(DefaultValue: "", Description: "Domain layer project file")]
    public FileSystemPath DomainLayerProjectFile;

    [SettingsEntry(DefaultValue: "", Description: "Shared libraries package names")]
    public string SharedLibrariesPackageNames;
}


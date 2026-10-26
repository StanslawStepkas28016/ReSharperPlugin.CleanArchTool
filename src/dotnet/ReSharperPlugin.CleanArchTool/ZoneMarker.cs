using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Rider.Backend.Env;

namespace ReSharperPlugin.CleanArchTool;

[ZoneDefinition(ZoneFlags.AutoEnable)]
// [ZoneDefinitionConfigurableFeature("Title", "Description", IsInProductSection: false)]
public interface ICleanArchToolZone : IZone,
    IRequire<IRiderPlatformZone>,
    IRequire<DaemonZone>
{
}

[ZoneMarker]
public class ZoneMarker : IRequire<ICleanArchToolZone>
{
}
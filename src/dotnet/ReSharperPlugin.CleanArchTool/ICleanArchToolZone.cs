using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Rider.Backend.Env;

namespace ReSharperPlugin.CleanArchTool;

public interface ICleanArchToolZone : IZone, IRequire<IRiderPlatformZone>, IRequire<DaemonZone>
{
}
using System.IO;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;

namespace ReSharperPlugin.CleanArchTool;

[DefaultSettings(typeof(CleanArchSettings))]
public class SettingsProvider : IHaveDefaultSettingsStream
{
    public string Name => "ReSharper SDK";

    public Stream GetDefaultSettingsStream(
        Lifetime lifetime
    )
    {
        var manifestResourceStream = typeof(SettingsProvider)
            .Assembly
            .GetManifestResourceStream(typeof(SettingsProvider).Namespace + ".CleanArchSettings.DotSettings")
            .NotNull();
        lifetime.OnTermination(manifestResourceStream);
        return manifestResourceStream;
    }
}
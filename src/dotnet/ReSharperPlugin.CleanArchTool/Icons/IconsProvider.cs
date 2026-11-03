using JetBrains.Application;
using JetBrains.IDE.UI;
using JetBrains.Rider.Model;
using JetBrains.UI.Icons;

namespace ReSharperPlugin.CleanArchTool.Icons;

[ShellComponent]
public class IconsProvider
{
    private readonly IIconHost _iconHost;

    public IconsProvider(
        IIconHost iconHost
    )
    {
        _iconHost = iconHost;
    }

    public IconModel GetIcon(
        IconId iconId
    )
    {
        return _iconHost.Transform(iconId);
    }
}
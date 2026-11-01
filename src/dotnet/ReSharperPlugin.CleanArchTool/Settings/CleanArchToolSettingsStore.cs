using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;

namespace ReSharperPlugin.CleanArchTool.Settings;

[ShellComponent]
public class CleanArchToolSettingsStore
{
    private readonly ISettingsStore _settingsStore;
    private readonly DataContexts _dataContexts;
    
    public CleanArchToolSettingsStore(
        ISettingsStore settingsStore,
        DataContexts dataContexts)
    {
        _settingsStore = settingsStore;
        _dataContexts = dataContexts;
    }

    public CleanArchToolSettings GetSettings()
    {
        var boundSettings = BindSettingsStore();
        return boundSettings.GetKey<CleanArchToolSettings>(SettingsOptimization.OptimizeDefault);;
    }

    public void SetSettings(CleanArchToolSettings settings)
    {
        var boundSettings = BindSettingsStore();
        boundSettings.SetKey(settings, SettingsOptimization.OptimizeDefault);
    }

    private IContextBoundSettingsStore BindSettingsStore()
    {
        var store = _settingsStore.BindToContextTransient(ContextRange.Smart((l, _) => _dataContexts.CreateOnSelection(l)));
        return store;
    }
}
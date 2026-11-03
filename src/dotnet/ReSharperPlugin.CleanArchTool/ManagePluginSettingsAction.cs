using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.DataFlow;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Extensions;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.UI.RichText;
using JetBrains.Util;
using ReSharperPlugin.CleanArchTool.Settings;

namespace ReSharperPlugin.CleanArchTool;

[Action
(
    typeof(Resources),
    nameof(Resources.SettingsTitle),
    DescriptionResourceName = nameof(Resources.SettingsDescription)
)]
public class ManagePluginSettingsAction :
    IExecutableAction
{
    public bool Update(
        IDataContext context,
        ActionPresentation presentation,
        DelegateUpdate nextUpdate
    )
    {
        return context.GetData(ProjectModelDataConstants.SOLUTION) != null;
    }

    public void Execute(
        IDataContext context,
        DelegateExecute nextExecute
    )
    {
        var solution = context.GetData(ProjectModelDataConstants.SOLUTION);
        if (solution == null) return;

        var dialogHost = context.GetComponent<IDialogHost>();
        var settingsStore = solution.GetComponent<CleanArchToolSettingsStore>();
        var settings = settingsStore.GetSettings();

        dialogHost.Show
        (
            lt =>
            {
                var maxClassLinesTextBox = BeControls.GetTextBox(lt, initialText: settings.MaxClassLines.ToString());
                var maxClassMethodsTextBox = BeControls.GetTextBox
                    (lt, initialText: settings.MaxClassMethods.ToString());
                var createConfigFileButton = CreateConfigurationFileButton(lt, settingsStore);

                var grid = BeControls
                    .GetGrid();

                grid
                    .AddElement(createConfigFileButton)
                    .AddElement(Resources.MaxClassLines.GetBeLabel())
                    .AddElement(maxClassLinesTextBox)
                    .AddElement(Resources.MaxClassMethods.GetBeLabel())
                    .AddElement(maxClassMethodsTextBox);

                bool TrySubmitData()
                {
                    if (!settingsStore.ConfigurationFileExists())
                    {
                        MessageBox.ShowError(Resources.ConfigurationFileMissing);
                        return false;
                    }

                    if (!int.TryParse(maxClassLinesTextBox.Text.Value, out var maxClassLines) || maxClassLines < 1)
                    {
                        MessageBox.ShowError(Resources.InvalidMaxClassLines);
                        return false;
                    }

                    if (!int.TryParse(maxClassMethodsTextBox.Text.Value, out var maxClassMethods) ||
                        maxClassMethods < 1)
                    {
                        MessageBox.ShowError(Resources.InvalidMaxClassMethods);
                        return false;
                    }

                    settings.MaxClassLines = maxClassLines;
                    settings.MaxClassMethods = maxClassMethods;
                    settingsStore.SetSettings(settings);
                    solution
                        .GetComponent<IDaemon>()
                        .Invalidate();

                    return true;
                }

                var dialog = BeControls
                    .GetDialog
                    (
                        grid,
                        Resources.SettingsTitle,
                        nameof(ManagePluginSettingsAction)
                    );

                return dialog
                    .WithCustomButton
                    (
                        "OK",
                        lt,
                        () =>
                        {
                            if (TrySubmitData()) dialog.Result.Value = DialogResult.OK;
                        },
                        BeCommonBehavior.DIALOG_ACCEPT_BUTTON,
                        resultAfterClick: DialogResult.NONE
                    )
                    .WithCancelButton(lt);
            },
            Lifetime.Eternal
        );
    }

    private static BeButton CreateConfigurationFileButton(
        Lifetime lifetime,
        CleanArchToolSettingsStore settingsStore
    )
    {
        var buttonText = new Property<string>(nameof(CreateConfigurationFileButton), Resources.CreateConfigurationFile);

        BeButton button = null;
        button = BeControls.GetButton
        (
            BeControls.GetRichText(buttonText, lifetime, JetFontStyles.Regular),
            lifetime,
            () =>
            {
                settingsStore.CreateConfigurationFile();
                MarkAsCreated(button, buttonText);
            }
        );

        if (settingsStore.ConfigurationFileExists()) MarkAsCreated(button, buttonText);

        return button;
    }

    private static void MarkAsCreated(
        BeButton button,
        IProperty<string> buttonText
    )
    {
        buttonText.Value = Resources.ConfigurationFileAlreadyCreated;
        button.Enabled.Value = false;
    }
}
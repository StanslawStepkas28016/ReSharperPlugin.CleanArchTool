using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.DataFlow;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Extensions.Properties;
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
                var configHeader = BeControls.GetHeader
                    (Resources.ConfigHeader, headerStyle: HeaderStyle.SEPARATOR);

                var createConfigFileButton = CreateConfigurationFileButton(lt, settingsStore);

                var analyzersHeader = BeControls.GetHeader
                    (Resources.AnalyzersHeader, headerStyle: HeaderStyle.SEPARATOR);

                var maxClassLinesTextBox = BeControls.GetTextBox(lt, initialText: settings.MaxClassLines.ToString());
                var maxClassLinesEnabledCheckBox = BeControls
                    .GetCheckBox
                    (
                        Resources.CheckBoxEnabled,
                        $"{nameof(ManagePluginSettingsAction)}.{nameof(settings.MaxClassLinesEnabled)}",
                        initialValue: settings.MaxClassLinesEnabled,
                        lifetime: lt
                    );
                maxClassLinesEnabledCheckBox
                    .Property
                    .Advise(lt, isChecked => maxClassLinesTextBox.Enabled.Value = isChecked == true);

                var maxClassMethodsTextBox = BeControls.GetTextBox
                    (lt, initialText: settings.MaxClassMethods.ToString());
                var maxClassMethodsEnabledCheckBox = BeControls
                    .GetCheckBox
                    (
                        Resources.CheckBoxEnabled,
                        $"{nameof(ManagePluginSettingsAction)}.{nameof(settings.MaxClassMethodsEnabled)}",
                        initialValue: settings.MaxClassMethodsEnabled,
                        lifetime: lt
                    );
                maxClassMethodsEnabledCheckBox
                    .Property
                    .Advise(lt, isChecked => maxClassMethodsTextBox.Enabled.Value = isChecked == true);

                var maxCtorArgumentsTextBox = BeControls.GetTextBox
                    (lt, initialText: settings.MaxCtorArguments.ToString());
                var maxCtorArgumentsEnabledCheckBox = BeControls
                    .GetCheckBox
                    (
                        Resources.CheckBoxEnabled,
                        $"{nameof(ManagePluginSettingsAction)}.{nameof(settings.MaxCtorArgumentsEnabled)}",
                        initialValue: settings.MaxCtorArgumentsEnabled,
                        lifetime: lt
                    );
                maxCtorArgumentsEnabledCheckBox
                    .Property
                    .Advise(lt, isChecked => maxCtorArgumentsTextBox.Enabled.Value = isChecked == true);

                var maxMethodArgumentsTextBox = BeControls.GetTextBox
                    (lt, initialText: settings.MaxMethodArguments.ToString());
                var maxMethodArgumentsEnabledCheckBox = BeControls
                    .GetCheckBox
                    (
                        Resources.CheckBoxEnabled,
                        $"{nameof(ManagePluginSettingsAction)}.{nameof(settings.MaxMethodArgumentsEnabled)}",
                        initialValue: settings.MaxMethodArgumentsEnabled,
                        lifetime: lt
                    );
                maxMethodArgumentsEnabledCheckBox
                    .Property
                    .Advise(lt, isChecked => maxMethodArgumentsTextBox.Enabled.Value = isChecked == true);
                var checkMultipleMethodsCheckBox = BeControls
                    .GetCheckBox
                    (
                        Resources.CheckMultipleMethods,
                        $"{nameof(ManagePluginSettingsAction)}.{nameof(settings.CheckMultipleMethods)}",
                        initialValue: settings.CheckMultipleMethods,
                        lifetime: lt
                    );
                maxMethodArgumentsEnabledCheckBox
                    .Property
                    .Advise(lt, isChecked => checkMultipleMethodsCheckBox.Enabled.Value = isChecked == true);

                var layersHeader = BeControls.GetHeader
                    (Resources.LayersHeader, headerStyle: HeaderStyle.SEPARATOR);
                
                var grid = BeControls
                    .GetGrid();

                grid
                    .AddElement(configHeader)
                    .AddElement(createConfigFileButton)
                    .AddElement(analyzersHeader)
                    .AddElement(Resources.MaxClassLines.GetBeLabel())
                    .AddElement(maxClassLinesTextBox)
                    .AddElement(maxClassLinesEnabledCheckBox)
                    .AddElement(Resources.MaxClassMethods.GetBeLabel())
                    .AddElement(maxClassMethodsTextBox)
                    .AddElement(maxClassMethodsEnabledCheckBox)
                    .AddElement(Resources.MaxCtorArguments.GetBeLabel())
                    .AddElement(maxCtorArgumentsTextBox)
                    .AddElement(maxCtorArgumentsEnabledCheckBox)
                    .AddElement(Resources.MaxMethodArguments.GetBeLabel())
                    .AddElement(maxMethodArgumentsTextBox)
                    .AddElement(maxMethodArgumentsEnabledCheckBox)
                    .AddElement(checkMultipleMethodsCheckBox)
                    .AddElement(layersHeader);

                bool TrySubmitData()
                {
                    if (!settingsStore.ConfigurationFileExists())
                    {
                        MessageBox.ShowError(Resources.ConfigurationFileMissing);
                        return false;
                    }

                    var maxClassLinesEnabled = maxClassLinesEnabledCheckBox.Property.Value == true;
                    var maxClassLines = settings.MaxClassLines;
                    if (maxClassLinesEnabled &&
                        (!int.TryParse(maxClassLinesTextBox.Text.Value, out maxClassLines) || maxClassLines < 1))
                    {
                        MessageBox.ShowError(Resources.InvalidMaxClassLines);
                        return false;
                    }

                    var maxClassMethodsEnabled = maxClassMethodsEnabledCheckBox.Property.Value == true;
                    var maxClassMethods = settings.MaxClassMethods;
                    if ((maxClassMethodsEnabled &&
                         !int.TryParse(maxClassMethodsTextBox.Text.Value, out maxClassMethods)) || maxClassMethods < 1)
                    {
                        MessageBox.ShowError(Resources.InvalidMaxClassMethods);
                        return false;
                    }

                    var maxCtorArgumentsEnabled = maxCtorArgumentsEnabledCheckBox.Property.Value == true;
                    var maxCtorArguments = settings.MaxCtorArguments;
                    if ((maxCtorArgumentsEnabled &&
                         !int.TryParse(maxCtorArgumentsTextBox.Text.Value, out maxCtorArguments)) ||
                        maxCtorArguments < 1)
                    {
                        MessageBox.ShowError(Resources.InvalidMaxCtorArguments);
                        return false;
                    }

                    var maxMethodArgumentsEnabled = maxMethodArgumentsEnabledCheckBox.Property.Value == true;
                    var maxMethodArguments = settings.MaxMethodArguments;
                    var checkMultipleMethodsEnabled = checkMultipleMethodsCheckBox.Property.Value == true;
                    if ((maxMethodArgumentsEnabled &&
                         !int.TryParse(maxMethodArgumentsTextBox.Text.Value, out maxMethodArguments)) ||
                        maxMethodArguments < 1)
                    {
                        MessageBox.ShowError(Resources.InvalidMaxMethodArguments);
                        return false;
                    }

                    settings.MaxClassLines = maxClassLines;
                    settings.MaxClassLinesEnabled = maxClassLinesEnabled;
                    settings.MaxClassMethods = maxClassMethods;
                    settings.MaxClassMethodsEnabled = maxClassMethodsEnabled;
                    settings.MaxCtorArguments = maxCtorArguments;
                    settings.MaxCtorArgumentsEnabled = maxCtorArgumentsEnabled;
                    settings.MaxMethodArguments = maxMethodArguments;
                    settings.MaxMethodArgumentsEnabled = maxMethodArgumentsEnabled;
                    settings.CheckMultipleMethods = checkMultipleMethodsEnabled;
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
                        nameof(ManagePluginSettingsAction),
                        isResizable: true,
                        size: BeControlSizes.GetCustomSize(600, 800),
                        layoutPersistenceMode: DialogLayoutPersistenceMode.POSITION
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
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Extensions;
using JetBrains.Lifetimes;
using JetBrains.Rider.Model.UIAutomation;
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
        return true;
    }

    public void Execute(
        IDataContext context,
        DelegateExecute nextExecute
    )
    {
        var dialogHost = context.GetComponent<IDialogHost>();

        var settingsStore = context.GetComponent<CleanArchToolSettingsStore>();
        var settings = settingsStore.GetSettings();

        BeTextBox textBox;
        dialogHost.Show
        (
            lt => BeControls
                .GetDialog
                (
                    textBox = BeControls.GetTextBox(lt, initialText: settings.MaxClassLines.ToString()),
                    Resources.SettingsTitle,
                    nameof(ManagePluginSettingsAction)
                )
                .WithOkButton
                (
                    lt,
                    () =>
                    {
                        settings.MaxClassLines = int.Parse(textBox.Text.Value);
                        settingsStore.SetSettings(settings);
                    }
                )
                .WithCancelButton(lt),
            Lifetime.Eternal
        );

        /*var dialogHost = context.GetComponent<IDialogHost>();

        /*if (!Validate(context, out var selectedNodeValue, out var errorMessage))
        {
            MessageBox.ShowError(errorMessage);
            return;
        }#1#



        BeSpinner classLenghtLimitSpinner;

        dialogHost
            .Show
            (
                lt =>
                          {
                              var grid = BeControls.GetAutoGrid();
                              classLenghtLimitSpinner = new BeSpinner(100, int.MaxValue, 15);
                              classLenghtLimitSpinner.Value.Value = settings.MaxClassLines;

                              grid.AddElement(BeControls.GetAutoGrid()
                                  .AddElements(BeControls.BeLabel(Resources.MaxClassLines), classLenghtLimitSpinner));

                              return BeControls
                                  .GetDialog
                                  (
                                      grid,
                                      Resources.SettingsTitle,
                                      nameof(ManagePluginSettingsAction)
                                  )
                                  .WithCustomButton("Close", lt, dialogButtonAction: BeCommonBehavior.CLOSE);
                          },
                          Lifetime.Eternal
            );*/
    }
}
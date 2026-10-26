using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Controls.FileSystem;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.DataFlow;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Options;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.UnitTestFramework.Resources;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.Util;

namespace ReSharperPlugin.CleanArchTool.Pages;

[OptionsPage
(
    PID,
    PageTitle,
    typeof(UnitTestingThemedIcons.Session),
    ParentId = CodeInspectionPage.PID
)]
public class MainPage : BeSimpleOptionsPage
{
    private const string PID = nameof(MainPage);
    private const string PageTitle = "ReSharper SDK";

    private readonly Lifetime _lifetime;

    public MainPage(
        Lifetime lifetime,
        OptionsPageContext optionsPageContext,
        OptionsSettingsSmartContext optionsSettingsSmartContext,
        IconHostBase iconHost,
        ICommonFileDialogs dialogs
    )
        : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
    {
        _lifetime = lifetime;

        AddKeyword("CleanArch", "Clean", "Preferences");
        AddText("This settings page is responsible for CleanArchitecture plugin settings.");
        AddCommentText("Values are saved in a .dotSettings file.");

        AddHeader("Analyzer Options");
        AddIntOption
        (
            (
                CleanArchSettings x
            ) => x.MaxClassLines,
            "Limit of lines in a class",
            minValue: 15
        );

        AddHeader("Layer names");
        AddText("Specify the .csproj files of your projects accordingly.");

        AddFileChooserOption
        (
            (
                CleanArchSettings x
            ) => x.PresentationLayerProjectFile,
            iconHost,
            dialogs,
            nameof(CleanArchSettings.PresentationLayerProjectFile),
            lifetime,
            optionsSettingsSmartContext
        );
        AddCommentText("Presentation layer .csproj file");

        AddFileChooserOption
        (
            (
                CleanArchSettings x
            ) => x.ApplicationLayerProjectFile,
            iconHost,
            dialogs,
            nameof(CleanArchSettings.ApplicationLayerProjectFile),
            lifetime,
            optionsSettingsSmartContext
        );
        AddCommentText("Application layer .csproj file");
        
        AddFileChooserOption
        (
            (
                CleanArchSettings x
            ) => x.InfrastructureLayerProjectFile,
            iconHost,
            dialogs,
            nameof(CleanArchSettings.InfrastructureLayerProjectFile),
            lifetime,
            optionsSettingsSmartContext
        );
        AddCommentText("Infrastructure layer .csproj file");
        
        AddFileChooserOption
        (
            (
                CleanArchSettings x
            ) => x.DomainLayerProjectFile,
            iconHost,
            dialogs,
            nameof(CleanArchSettings.DomainLayerProjectFile),
            lifetime,
            optionsSettingsSmartContext
        );
        AddCommentText("Domain layer .csproj file");
        
        AddHeader("Shared libraries");
        AddText
        (
            "Specify the shared package names of your projects accordingly. Package names should be delimited with \";\"."
        );
        AddCommentText("Example: \"Microsoft.AspNetCore.Authentication.JwtBearer;Microsoft.AspNetCore.OpenApi\".");
        AddTextBox
        (
            (
                CleanArchSettings x
            ) => x.SharedLibrariesPackageNames,
            "Presentation layer .csproj filename"
        );
    }

    private BeControl AddFileChooserOption<TKeyClass>(
        Expression<Func<TKeyClass, FileSystemPath>> lambdaExpression,
        IconHostBase iconHost,
        ICommonFileDialogs dialogs,
        string propertyId,
        Lifetime lifetime,
        OptionsSettingsSmartContext optionsSettingsSmartContext,
        string toolTipText = null
    )
    {
        var property = new Property<FileSystemPath>(propertyId);

        optionsSettingsSmartContext.SetBinding
        (
            lifetime,
            lambdaExpression,
            property
        );

        return AddFileChooserOption
        (
            property,
            toolTipText,
            FileSystemPath.Empty,
            iconHost,
            dialogs,
            canBeEmpty: false,
            predefinedValues: null
        );
    }
}
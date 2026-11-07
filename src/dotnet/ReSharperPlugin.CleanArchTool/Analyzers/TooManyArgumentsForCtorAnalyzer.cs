using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.CleanArchTool.Settings;
using ReSharperPlugin.CleanArchTool.Warnings;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

/// <summary>
///     Responsible for analyzing the count of arguments within a c-tor of a class.
///     If the analyzed c-tor exceeds the user-set limit, the c-tor name will be highlighted.
/// </summary>
[ElementProblemAnalyzer
(
    typeof(IConstructorDeclaration),
    HighlightingTypes =
        [typeof(ConstructorDeclarationWarning)]
)]
public class TooManyArgumentsForCtorAnalyzer : ElementProblemAnalyzer<IConstructorDeclaration>
{
    protected override void Run(
        IConstructorDeclaration constructorDeclaration,
        ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer
    )
    {
        var argumentsCount = constructorDeclaration.ParameterDeclarations.Count;

        var settings = constructorDeclaration
            .GetSolution()
            .GetComponent<CleanArchToolSettingsStore>()
            .GetSettings();

        var limit = Math.Max(1, settings.MaxCtorArguments);

        if (argumentsCount <= limit) return;

        consumer.AddHighlighting
        (
            new ConstructorDeclarationWarning
            (
                constructorDeclaration,
                constructorDeclaration.TypeName.GetDocumentRange(),
                $"{Resources.PluginName}: {Resources.MaxCtorArgumentsWarning} {limit}."
            )
        );
    }
}
using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.CleanArchTool.Settings;
using ReSharperPlugin.CleanArchTool.Warnings;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

/// <summary>
///     Responsible for analyzing the count of the methods inside a class.
///     If the analyzed class exceeds the user-set limit, the class name will be highlighted.
/// </summary>
[ElementProblemAnalyzer
(
    typeof(IClassLikeDeclaration),
    HighlightingTypes = [typeof(ClassLikeDeclarationWarning)]
)]
public class TooManyMethodsClassAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration>
{
    protected override void Run(
        IClassLikeDeclaration classLikeDeclaration,
        ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer
    )
    {
        var settings = classLikeDeclaration
            .GetSolution()
            .GetComponent<CleanArchToolSettingsStore>()
            .GetSettings();

        if (!settings.MaxClassMethodsEnabled) return;

        var methodDeclarations = classLikeDeclaration.MethodDeclarations;
        var methodDeclarationsCount = methodDeclarations.Count;

        var limit = Math.Max(1, settings.MaxClassMethods);

        if (methodDeclarationsCount <= limit) return;

        consumer.AddHighlighting
        (
            new ClassLikeDeclarationWarning
            (
                classLikeDeclaration,
                classLikeDeclaration.NameIdentifier.GetDocumentRange(),
                $"{Resources.PluginName}: {Resources.MaxClassMethodsWarning} {limit}."
            )
        );
    }
}
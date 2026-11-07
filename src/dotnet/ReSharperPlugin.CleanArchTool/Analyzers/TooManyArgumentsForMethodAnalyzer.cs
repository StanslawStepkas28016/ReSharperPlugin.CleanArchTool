using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.Logging;
using ReSharperPlugin.CleanArchTool.Settings;
using ReSharperPlugin.CleanArchTool.Warnings;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

[ElementProblemAnalyzer
(
    typeof(IClassLikeDeclaration),
    HighlightingTypes =
        [typeof(MethodDeclarationWarning)]
)]
public class TooManyArgumentsForMethodAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration>
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

        var limit = Math.Max(1, settings.MaxMethodArguments);

        var methodDeclarationsEnumerable = classLikeDeclaration.MethodDeclarationsEnumerable;

        foreach (var methodDeclaration in methodDeclarationsEnumerable)
        {
            if (methodDeclaration.ParameterDeclarations.Count > limit)
            {
                consumer.AddHighlighting
                (
                    new MethodDeclarationWarning
                    (
                        methodDeclaration,
                        methodDeclaration.NameIdentifier.GetDocumentRange(),
                        $"{Resources.PluginName}: {Resources.MaxMethodArgumentsWarning} {limit}."
                    )
                );
                
                if (!settings.CheckMultipleMethods) return;
            }
        }
    }
}
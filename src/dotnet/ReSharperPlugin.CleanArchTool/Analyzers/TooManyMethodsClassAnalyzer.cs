using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.CleanArchTool.Settings;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

/// <summary>
///     Responsible for analyzing the count of the methods inside a class.
///     If the analyzed class exceeds the user-set limit, the class name will be highlighted.
/// </summary>
[ElementProblemAnalyzer
(
    typeof(IClassLikeDeclaration),
    HighlightingTypes = [typeof(TooManyMethodsClassWarning)]
)]
public class TooManyMethodsClassAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration>
{
    protected override void Run(
        IClassLikeDeclaration classLikeDeclaration,
        ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer
    )
    {
        var methodDeclarations = classLikeDeclaration.MethodDeclarations;
        var methodDeclarationsCount = methodDeclarations.Count;

        var settings = classLikeDeclaration
            .GetSolution()
            .GetComponent<CleanArchToolSettingsStore>()
            .GetSettings();

        var maxClassMethods = System.Math.Max(1, settings.MaxClassMethods);
        
        if (methodDeclarationsCount <= maxClassMethods) return;

        consumer.AddHighlighting
        (
            new TooManyMethodsClassWarning
            (
                classLikeDeclaration,
                classLikeDeclaration.NameIdentifier.GetDocumentRange(),
                maxClassMethods
            )
        );
    }
}
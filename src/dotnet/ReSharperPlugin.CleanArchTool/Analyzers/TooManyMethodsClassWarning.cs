using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class TooManyMethodsClassWarning : IHighlighting
{
    private readonly IClassLikeDeclaration _classLikeDeclaration;
    private readonly DocumentRange _documentRange;

    public TooManyMethodsClassWarning(
        IClassLikeDeclaration classLikeDeclaration,
        DocumentRange documentRange,
        int limit
    )
    {
        _classLikeDeclaration = classLikeDeclaration;
        _documentRange = documentRange;
        ToolTip = $"{Resources.MaxClassMethodsWarning} {limit}.";
    }

    public bool IsValid()
    {
        return _classLikeDeclaration.IsValid();
    }

    public DocumentRange CalculateRange()
    {
        return _documentRange;
    }

    public string ToolTip { get; }
    public string ErrorStripeToolTip => ToolTip;
}
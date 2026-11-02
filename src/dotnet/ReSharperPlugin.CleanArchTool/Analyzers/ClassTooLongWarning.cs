using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.CodeInspections;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class ClassTooLongWarning : IHighlighting
{
    private readonly DocumentRange _documentRange;
    private readonly IClassLikeDeclaration _classLikeDeclaration;

    public ClassTooLongWarning(
        IClassLikeDeclaration classLikeDeclaration,
        DocumentRange documentRange,
        int limit
    )
    {
        _classLikeDeclaration = classLikeDeclaration;
        _documentRange = documentRange;
        ToolTip = $"{Resources.MaxClassLines} {limit}.";
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
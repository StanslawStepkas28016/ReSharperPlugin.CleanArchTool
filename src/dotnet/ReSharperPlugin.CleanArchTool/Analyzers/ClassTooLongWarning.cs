using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.CodeInspections;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class ClassTooLongWarning : IHighlighting
{
    private readonly DocumentRange _documentRange;
    public readonly IClassLikeDeclaration ClassLikeDeclaration;

    public ClassTooLongWarning(
        IClassLikeDeclaration classLikeDeclaration,
        DocumentRange documentRange,
        int limit
    )
    {
        ClassLikeDeclaration = classLikeDeclaration;
        _documentRange = documentRange;
        ToolTip = $"Class exceeds the line limit. Current line limit is {limit}";
    }

    public bool IsValid()
    {
        return ClassLikeDeclaration.IsValid();
    }

    public DocumentRange CalculateRange()
    {
        return _documentRange;
    }

    public string ToolTip { get; }
    public string ErrorStripeToolTip => ToolTip;
}
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.Warnings;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class ClassLikeDeclarationWarning : IHighlighting
{
    private readonly DocumentRange _documentRange;
    private readonly IClassLikeDeclaration _classLikeDeclaration;

    public ClassLikeDeclarationWarning(
        IClassLikeDeclaration classLikeDeclaration,
        DocumentRange documentRange,
        string toolTipString
    )
    {
        _classLikeDeclaration = classLikeDeclaration;
        _documentRange = documentRange;
        ToolTip = toolTipString;
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
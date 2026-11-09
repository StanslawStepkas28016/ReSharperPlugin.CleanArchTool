using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.Warnings;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class MethodDeclarationWarning : IHighlighting
{
    private readonly IMethodDeclaration _methodDeclaration;
    private readonly DocumentRange _documentRange;

    public MethodDeclarationWarning(
        IMethodDeclaration methodDeclaration,
        DocumentRange documentRange,
        string toolTipResource
    )
    {
        _methodDeclaration = methodDeclaration;
        _documentRange = documentRange;
        ToolTip = toolTipResource;
    }

    public bool IsValid()
    {
        return _methodDeclaration.IsValid();
    }

    public DocumentRange CalculateRange()
    {
        return _documentRange;
    }

    public string ToolTip { get; }
    public string ErrorStripeToolTip => ToolTip;
}
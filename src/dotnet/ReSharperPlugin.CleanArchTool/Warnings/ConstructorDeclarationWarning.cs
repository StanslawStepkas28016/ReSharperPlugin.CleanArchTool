using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperPlugin.CleanArchTool.Warnings;

[StaticSeverityHighlighting(Severity.WARNING, typeof(HighlightingGroupIds.GutterMarks))]
public class ConstructorDeclarationWarning : IHighlighting
{
    private readonly IConstructorDeclaration _constructorDeclaration;
    private readonly DocumentRange _documentRange;

    public ConstructorDeclarationWarning(
        IConstructorDeclaration constructorDeclaration,
        DocumentRange documentRange,
        string toolTipString
    )
    {
        _constructorDeclaration = constructorDeclaration;
        _documentRange = documentRange;
        ToolTip = toolTipString;
    }

    public bool IsValid()
    {
        return _constructorDeclaration.IsValid();
    }

    public DocumentRange CalculateRange()
    {
        return _documentRange;
    }

    public string ToolTip { get; }
    public string ErrorStripeToolTip => ToolTip;
}
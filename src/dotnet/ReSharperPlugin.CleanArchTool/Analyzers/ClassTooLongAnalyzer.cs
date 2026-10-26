using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.CleanArchTool.CodeInspections;

namespace ReSharperPlugin.CleanArchTool.Analyzers;

/// <summary>
///     Responsible for analyzing the count of lines inside a class.
///     If the analyzed class exceeds the user-set limit, the class name will be highlighted.
/// </summary>
[ElementProblemAnalyzer
(
    typeof(IClassLikeDeclaration),
    HighlightingTypes =
        new[] { typeof(ClassTooLongWarning) }
)]
public class ClassTooLongAnalyzer : ElementProblemAnalyzer<IClassLikeDeclaration>
{
    protected override void Run(
        IClassLikeDeclaration classLikeDeclaration,
        ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer
    )
    {
        var classBody = classLikeDeclaration.Body;
        if (classBody == null) return;

        var leftBrace = classBody.LBrace?.GetDocumentRange() ?? DocumentRange.InvalidRange;
        var rightBrace = classBody.RBrace?.GetDocumentRange() ?? DocumentRange.InvalidRange;
        if (!leftBrace.IsValid() || !rightBrace.IsValid()) return;

        var document = leftBrace.Document;

        var startLine = document
            .GetDocumentStartOffset()
            .ToDocumentCoords()
            .Line;

        var endLine = document
            .GetDocumentEndOffset()
            .ToDocumentCoords()
            .Line;

        var lineCount = (int)(endLine - startLine) - 1;

        var limit = Math.Max
        (
            1,
            data.SettingsStore.GetValue
            ((
                    CleanArchSettings s
                ) => s.MaxClassLines
            )
        );

        if (lineCount <= limit) return;

        consumer.AddHighlighting
        (
            new ClassTooLongWarning
            (
                classLikeDeclaration,
                classLikeDeclaration.NameIdentifier.GetDocumentRange(),
                limit
            )
        );
    }
}
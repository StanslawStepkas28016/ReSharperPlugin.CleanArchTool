using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.FeaturesTestFramework.Daemon;
using JetBrains.ReSharper.Psi;
using NUnit.Framework;
using ReSharperPlugin.CleanArchTool.CodeInspections;

namespace ReSharperPlugin.CleanArchTool.Tests.Analyzers;

[TestFixture]
public class ClassTooLongAnalyzerTests : CSharpHighlightingTestBase
{
    protected override string RelativeTestDataPath => "Analyzers/ClassTooLong";

    protected override bool HighlightingPredicate(
        IHighlighting highlighting,
        IPsiSourceFile sourceFile,
        IContextBoundSettingsStore settingsStore
    )
    {
        return highlighting is ClassTooLongWarning;
    }

    [Test]
    public void ShortClass()
    {
        DoNamedTest();
    }

    [Test]
    public void LongClass()
    {
        DoNamedTest();
    }
}
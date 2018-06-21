using Xunit;

namespace DotNetDxc.Tests
{
    public class DotNetDxcTests
    {
        public DotNetDxcTests()
        {
            HlslDxcLib.DxcCreateInstanceFn = DefaultDxcLib.GetDxcCreateInstanceFn();
        }

        private static IDxcTranslationUnit ParseTranslationUnit(string path)
        {
            var dxcIntelliSense = HlslDxcLib.CreateDxcIntelliSense();

            var index = dxcIntelliSense.CreateIndex();

            var commandLineArgs = new string[0];

            var unsavedFiles = new IDxcUnsavedFile[]
            {
                new TrivialDxcUnsavedFile(path, System.IO.File.ReadAllText(path))
            };

            var translationUnit = index.ParseTranslationUnit(
                path,
                commandLineArgs,
                commandLineArgs.Length,
                unsavedFiles,
                (uint) unsavedFiles.Length,
                (uint) DxcTranslationUnitFlags.DxcTranslationUnitFlags_UseCallerThread);

            Assert.NotNull(translationUnit);

            return translationUnit;
        }

        [Fact]
        public void CanParseHlsl()
        {
            var translationUnit = ParseTranslationUnit("simple.hlsl");

            Assert.Equal(0u, translationUnit.GetNumDiagnostics());
        }

        [Fact]
        public void CanGetCursorInformation()
        {
            var translationUnit = ParseTranslationUnit("simple.hlsl");

            var file = translationUnit.GetFile("simple.hlsl");
            var locationOfPositionFieldUsage = translationUnit.GetLocation(file, 11, 15);
            var cursor = translationUnit.GetCursorForLocation(locationOfPositionFieldUsage);

            Assert.Equal(DxcCursorKind.DxcCursor_MemberRefExpr, cursor.GetCursorKind());
            Assert.Equal(DxcCursorKindFlags.Expression, cursor.GetCursorKindFlags());
            Assert.Equal("float4", cursor.GetCursorType().GetSpelling());

            cursor.GetDefinitionCursor().GetLocation().GetSpellingLocation(out _, out var defLine, out var defCol, out var defOffset);
            Assert.Equal(3u, defLine);
            Assert.Equal(9u, defCol);
        }
    }
}

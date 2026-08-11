using ProcessamentoEmLote.Services;

namespace ProcessamentoEmLote.Tests
{
    public class ProcessingServiceTests
    {
        private static string GetProjectPath(string relativePath)
        {
            var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            return Path.GetFullPath(Path.Combine(projectRoot, relativePath));
        }

        [Fact]
        public void DeveExecutarPipelineSemErros()
        {
            var reader = new JsonReaderService();
            var outputDir = GetProjectPath("Data/TestOutput");
            var writer = new CsvWriterService(outputDir);
            var processor = new ProcessingService(reader, writer);

            var inputFile = GetProjectPath("Data/Input/sample_clubes.jsonl");
            processor.Run(inputFile);

            Assert.True(File.Exists(Path.Combine(outputDir, "clubs.csv")));
            Assert.True(File.Exists(Path.Combine(outputDir, "players.csv")));
        }
    }
}

using ProcessamentoEmLote.Services;
using ProcessamentoEmLote.Utils;

namespace ProcessamentoEmLote.Tests
{
    public class ProcessingServiceTests
    {
        [Fact]
        public void DeveExecutarPipelineSemErros()
        {
            var reader = new JsonReaderService();
            var outputDir = CreateUniqueOutputDir();
            var writer = new CsvWriterService(outputDir);
            var processor = new ProcessingService(reader, writer);

            var inputFile = StringUtils.GetProjectPath("Data/Input/sample_clubes.jsonl");

            processor.Run(inputFile);

            Assert.True(Directory.Exists(outputDir));
        }

        private static string CreateUniqueOutputDir()
        {
            var dir = Path.Combine(Path.GetTempPath(), "ProcessamentoEmLote.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }
    }
}

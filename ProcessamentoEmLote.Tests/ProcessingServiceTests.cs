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
            var outputDir = StringUtils.GetProjectPath("Data/TestOutput");
            var writer = new CsvWriterService(outputDir);
            var processor = new ProcessingService(reader, writer);

            var inputFile = StringUtils.GetProjectPath("Data/Input/sample_clubes.jsonl");

            // Executa sem lançar exceção
            processor.Run(inputFile);

            // Garante que a pasta de saída existe
            Assert.True(Directory.Exists(outputDir));
        }

    }
}

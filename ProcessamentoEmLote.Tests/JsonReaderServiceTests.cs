using ProcessamentoEmLote.Services;

namespace ProcessamentoEmLote.Tests
{
    public class JsonReaderServiceTests
    {
        private static string GetProjectPath(string relativePath)
        {
            var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            return Path.GetFullPath(Path.Combine(projectRoot, relativePath));
        }

        [Fact]
        public void DeveIgnorarArquivoComExtensaoInvalida()
        {
            var reader = new JsonReaderService();
            Assert.Throws<NotSupportedException>(() => reader.ReadClubs("arquivo.txt").ToList());
        }

        [Fact]
        public void DeveIgnorarArquivoVazio()
        {
            var reader = new JsonReaderService();
            var filePath = GetProjectPath("Data/Input/arquivo_vazio.jsonl");

            // Garante que o arquivo vazio existe
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }

            var result = reader.ReadClubs(filePath).ToList();
            Assert.Empty(result);
        }

        [Fact]
        public void DeveLerClubesValidos()
        {
            var reader = new JsonReaderService();
            var filePath = GetProjectPath("Data/Input/sample_clubes.jsonl");

            var result = reader.ReadClubs(filePath).ToList();

            Assert.NotEmpty(result);
            Assert.All(result, c => Assert.False(string.IsNullOrWhiteSpace(c.ClubId)));
        }
    }
}

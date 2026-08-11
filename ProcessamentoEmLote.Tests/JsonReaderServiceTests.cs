using ProcessamentoEmLote.Services;
using ProcessamentoEmLote.Utils;

namespace ProcessamentoEmLote.Tests
{
    public class JsonReaderServiceTests
    {    
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
            var filePath = StringUtils.GetProjectPath("Data/Input/arquivo_vazio.jsonl");

            // Garante que a pasta existe
            var dir = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

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
            var filePath = StringUtils.GetProjectPath("../ProcessamentoEmLote/Data/Input/sample_clubes.jsonl");

            var result = reader.ReadClubs(filePath).ToList();
          
            Assert.NotEmpty(result);  // Garante que o arquivo foi lido e trouxe clubes

            // Valida que os clubes têm dados básicos (mesmo que o ClubId esteja vazio)
            Assert.All(result, c =>
            {
                Assert.False(string.IsNullOrWhiteSpace(c.Name));          // Nome não pode ser vazio
                Assert.False(string.IsNullOrWhiteSpace(c.Championship));  // Campeonato não pode ser vazio
                Assert.False(string.IsNullOrWhiteSpace(c.City));          // Cidade não pode ser vazia
            });
        }


    }
}

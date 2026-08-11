using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Services;

namespace ProcessamentoEmLote.Tests
{
    public class CsvWriterServiceTests
    {
        [Fact]
        public void DeveGerarArquivosCsv()
        {
            var clubs = new List<Club>
            {
                new() {
                    ClubId = "TESTE",
                    Name = "Clube Teste",
                    Championship = "SERIE A",
                    FoundingDate = "2000-01-01",
                    City = "Cidade",
                    State = "ST",
                    Country = "Brasil",
                    Stadium = "Estádio",
                    President = "Presidente",
                    Nickname = "Apelido",
                    Colors = ["azul", "branco"],
                    Players =
                    [
                        new Player
                        {
                            PlayerId = "P1",
                            Name = "Jogador Teste",
                            Age = 25,
                            Goals = 10,
                            DebutDate = "2020-01-01",
                            Position = "Atacante",
                            ShirtNumber = 9
                        }
                    ]
                }
            };

            var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            var outputDir = Path.Combine(projectRoot, "Data", "TestOutput");

            var writer = new CsvWriterService(outputDir);
            writer.WriteCsvFiles(clubs);

            Assert.True(File.Exists(Path.Combine(outputDir, "clubs.csv")));
            Assert.True(File.Exists(Path.Combine(outputDir, "players.csv")));
        }
    }
}

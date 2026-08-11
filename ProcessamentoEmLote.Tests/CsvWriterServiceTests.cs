using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Services;
using ProcessamentoEmLote.Utils;
using Xunit;

namespace ProcessamentoEmLote.Tests
{
    public class CsvWriterServiceTests
    {
        [Fact]
        public void DeveGerarArquivosCsv()
        {
            var clubs = new List<Club>
            {
                new Club {
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
                    Colors = new List<string> { "azul", "branco" },
                    Players = new List<Player>
                    {
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
                    }
                }
            };

            // Usa o utilitário centralizado
            var outputDir = StringUtils.GetProjectPath("Data/TestOutput");

            var writer = new CsvWriterService(outputDir);
            writer.WriteCsvFiles(clubs);

            Assert.True(File.Exists(Path.Combine(outputDir, "clubs.csv")));
            Assert.True(File.Exists(Path.Combine(outputDir, "players.csv")));
        }
    }
}

using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;
using System.Numerics;

namespace ProcessamentoEmLote.Tests
{
    public class ClubTests
    {
        [Fact]
        public void DeveCriarClubeComDadosValidos()
        {
            var club = new Club
            {
                ClubId = "SCCP",
                Name = "Corinthians",
                Championship = "SERIE A",
                FoundingDate = "1910-09-01",
                City = "São Paulo",
                State = "SP",
                Country = "Brasil",
                Stadium = "Neo Química Arena",
                President = "Augusto Melo",
                Nickname = "Timão",
                Colors = ["preto", "branco"]
            };

            Assert.Equal("SCCP", club.ClubId);
            Assert.Equal("Corinthians", club.Name);
            Assert.Contains("preto", club.Colors);
        }

        [Fact]
        public void DeveIgnorarDatasInvalidas()
        {
            var parsedDate = DateUtils.ParseDate("data-invalida");

            Assert.Equal(string.Empty, parsedDate);
        }

        [Fact]
        public void DevePermitirListaDeJogadores()
        {
            var club = new Club
            {
                ClubId = "SCCP",
                Players =
                [
                    new Player { PlayerId = "P1", Name = "Jogador Teste", Age = 25 }
                ]
            };

            Assert.Single(club.Players);
            Assert.Equal("Jogador Teste", club.Players.First().Name);
        }
    }
}

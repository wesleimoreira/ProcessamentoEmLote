using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;

namespace ProcessamentoEmLote.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void DeveCriarJogadorComDadosValidos()
        {
            var player = new Player
            {
                PlayerId = "P1",
                Name = "Jogador Teste",
                Age = 25,
                Goals = 10,
                DebutDate = "2020-01-01",
                Position = "Atacante",
                ShirtNumber = 9,
                Nationality = "Brasil",
                MarketValue = 1000000
            };

            Assert.Equal("P1", player.PlayerId);
            Assert.Equal("Jogador Teste", player.Name);
            Assert.Equal(25, player.Age);
            Assert.Equal(10, player.Goals);
            Assert.Equal("Atacante", player.Position);
            Assert.Equal(9, player.ShirtNumber);
        }

        [Fact]
        public void DevePermitirValoresNulos()
        {
            var player = new Player
            {
                PlayerId = "P2",
                Name = "Jogador Sem Dados"
            };

            Assert.Equal("P2", player.PlayerId);
            Assert.Equal("Jogador Sem Dados", player.Name);
            Assert.Null(player.Age);
            Assert.Null(player.Goals);
            Assert.Null(player.ShirtNumber);
        }

        [Fact]
        public void DeveIgnorarDataDeEstreiaInvalida()
        {
            var player = new Player
            {
                PlayerId = "P3",
                Name = "Jogador Data Inválida",
                DebutDate = "data-invalida"
            };

            var parsedDate = DateUtils.ParseDate(player.DebutDate);

            Assert.Equal(string.Empty, parsedDate);
        }

        [Fact]
        public void DeveAceitarValoresDeMercadoGrandes()
        {
            var player = new Player
            {
                PlayerId = "P4",
                Name = "Jogador Caro",
                MarketValue = 50000000 // 50 milhões
            };

            Assert.Equal(50000000, player.MarketValue);
        }
    }
}

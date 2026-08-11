using CsvHelper;
using CsvHelper.Configuration;
using ProcessamentoEmLote.DTOs;
using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;
using System.Globalization;

namespace ProcessamentoEmLote.Services
{
    public class CsvWriterService
    {
        private readonly string _outputDir;

        public CsvWriterService(string outputDir = "Data/Output")
        {
            _outputDir = StringUtils.GetProjectPath(outputDir);

            if (!Directory.Exists(_outputDir))
            {
                Directory.CreateDirectory(_outputDir);
            }
        }



        public void WriteCsvFiles(IEnumerable<Club> clubs)
        {
            if (clubs == null || !clubs.Any())
            {
                Logger.Warn("Nenhum clube válido encontrado para exportação.");
                return;
            }

            var clubsFile = Path.Combine(_outputDir, "clubs.csv");
            var playersFile = Path.Combine(_outputDir, "players.csv");

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = System.Text.Encoding.UTF8,
                HasHeaderRecord = true,
                Quote = '"',
                Escape = '"',
                BadDataFound = context =>
                {
                    Logger.Warn($"Dado inválido ignorado: {context.RawRecord}");
                }
            };

            // Escreve clubs.csv
            using (var writer = new StreamWriter(clubsFile))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteHeader<ClubCsv>();
                csv.NextRecord();

                foreach (var club in clubs.Where(c => IsValidChampionship(c.Championship)))
                {
                    var record = MapClubToCsv(club);
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }
            }

            // Escreve players.csv
            using (var writer = new StreamWriter(playersFile))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteHeader<PlayerCsv>();
                csv.NextRecord();

                foreach (var club in clubs.Where(c => IsValidChampionship(c.Championship)))
                {
                    if (club.Players == null || !club.Players.Any()) continue;

                    foreach (var player in club.Players)
                    {
                        var record = MapPlayerToCsv(club.ClubId, player);
                        csv.WriteRecord(record);
                        csv.NextRecord();
                    }
                }
            }

            Logger.Info($"Arquivos gerados em {_outputDir}");
        }

        private static bool IsValidChampionship(string championship)
        {
            var normalized = StringUtils.NormalizeUpper(championship);
            return normalized == "SERIE A" || normalized == "SERIE B";
        }

        private static ClubCsv MapClubToCsv(Club club)
        {
            return new ClubCsv
            {
                IdDoClube = StringUtils.Safe(club.ClubId),
                Nome = StringUtils.Safe(club.Name),
                Campeonato = StringUtils.Safe(club.Championship),
                DataDeFundacao = DateUtils.ParseDate(club.FoundingDate),
                Cidade = StringUtils.Safe(club.City),
                Estado = StringUtils.Safe(club.State),
                Pais = StringUtils.Safe(club.Country),
                Estadio = StringUtils.Safe(club.Stadium),
                Presidente = StringUtils.Safe(club.President),
                Apelido = StringUtils.Safe(club.Nickname),
                Cores = StringUtils.JoinWithPipe(club.Colors)
            };
        }

        private static PlayerCsv MapPlayerToCsv(string clubId, Player player)
        {
            return new PlayerCsv
            {
                IdDoClube = StringUtils.Safe(clubId),
                IdDoJogador = StringUtils.Safe(player.PlayerId),
                Nome = StringUtils.Safe(player.Name),
                Idade = player.Age?.ToString() ?? string.Empty,
                Gols = player.Goals?.ToString() ?? string.Empty,
                DataDeEstreia = DateUtils.ParseDate(player.DebutDate),
                Posicao = StringUtils.Safe(player.Position),
                NumeroDaCamisa = player.ShirtNumber?.ToString() ?? string.Empty
            };
        }

    }
}

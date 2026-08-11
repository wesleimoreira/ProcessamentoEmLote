using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;
using System.Text;

namespace ProcessamentoEmLote.Services
{
    public class CsvWriterService_s
    {
        private readonly string _outputDir;

        public CsvWriterService_s(string outputDir = "Data/Output")
        {
            var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");

            _outputDir = Path.GetFullPath(Path.Combine(projectRoot, outputDir));

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

            WriteClubsCsv(clubs, clubsFile);
            WritePlayersCsv(clubs, playersFile);

            Logger.Info($"Arquivos gerados em {_outputDir}");
        }

        private static void WriteClubsCsv(IEnumerable<Club> clubs, string filePath)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine("Id do Clube,Nome,Campeonato,Data de Fundação,Cidade,Estado,País,Estádio,Presidente,Apelido,Cores");

            foreach (var club in clubs.Where(c => IsValidChampionship(c.Championship)))
            {
                var line = string.Join(",",
                    StringUtils.EscapeCsv(club.ClubId),
                    StringUtils.EscapeCsv(club.Name),
                    StringUtils.EscapeCsv(club.Championship),
                    StringUtils.EscapeCsv(DateUtils.ParseDate(club.FoundingDate)),
                    StringUtils.EscapeCsv(club.City),
                    StringUtils.EscapeCsv(club.State),
                    StringUtils.EscapeCsv(club.Country),
                    StringUtils.EscapeCsv(club.Stadium),
                    StringUtils.EscapeCsv(club.President),
                    StringUtils.EscapeCsv(club.Nickname),
                    StringUtils.EscapeCsv(StringUtils.JoinWithPipe(club.Colors))
                );

                writer.WriteLine(line);
            }
        }

        private static void WritePlayersCsv(IEnumerable<Club> clubs, string filePath)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            writer.WriteLine("Id do Clube,Id do Jogador,Nome,Idade,Gols,Data de Estreia,Posição,Número da Camisa");

            foreach (var club in clubs.Where(c => IsValidChampionship(c.Championship)))
            {
                if (club.Players == null || club.Players.Count == 0) continue;

                foreach (var player in club.Players)
                {
                    var line = string.Join(",",
                        StringUtils.EscapeCsv(club.ClubId),
                        StringUtils.EscapeCsv(player.PlayerId),
                        StringUtils.EscapeCsv(player.Name),
                        StringUtils.EscapeCsv(player.Age?.ToString() ?? string.Empty),
                        StringUtils.EscapeCsv(player.Goals?.ToString() ?? string.Empty),
                        StringUtils.EscapeCsv(DateUtils.ParseDate(player.DebutDate)),
                        StringUtils.EscapeCsv(player.Position),
                        StringUtils.EscapeCsv(player.ShirtNumber?.ToString() ?? string.Empty)
                    );

                    writer.WriteLine(line);
                }
            }
        }

        private static bool IsValidChampionship(string championship)
        {
            var normalized = StringUtils.NormalizeUpper(championship);
            return normalized == "SERIE A" || normalized == "SERIE B";
        }
    }
}

using ProcessamentoEmLote.DTOs;
using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;
using System.Text;

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

        public bool WriteCsvFiles(IEnumerable<Club> clubs)
        {
            if (clubs == null)
            {
                Logger.Warn("Nenhum clube válido encontrado para exportação.");
                return false;
            }

            var clubsFile = Path.Combine(_outputDir, "clubs.csv");
            var playersFile = Path.Combine(_outputDir, "players.csv");

            var clubCount = 0;
            var playerCount = 0;

            using (var clubsWriter = new StreamWriter(clubsFile, false, Encoding.UTF8))
            {
                WriteRow(clubsWriter,
                    nameof(ClubCsv.IdDoClube),
                    nameof(ClubCsv.Nome),
                    nameof(ClubCsv.Campeonato),
                    nameof(ClubCsv.DataDeFundacao),
                    nameof(ClubCsv.Cidade),
                    nameof(ClubCsv.Estado),
                    nameof(ClubCsv.Pais),
                    nameof(ClubCsv.Estadio),
                    nameof(ClubCsv.Presidente),
                    nameof(ClubCsv.Apelido),
                    nameof(ClubCsv.Cores));

                using (var playersWriter = new StreamWriter(playersFile, false, Encoding.UTF8))
                {
                    WriteRow(playersWriter,
                        nameof(PlayerCsv.IdDoClube),
                        nameof(PlayerCsv.IdDoJogador),
                        nameof(PlayerCsv.Nome),
                        nameof(PlayerCsv.Idade),
                        nameof(PlayerCsv.Gols),
                        nameof(PlayerCsv.DataDeEstreia),
                        nameof(PlayerCsv.Posicao),
                        nameof(PlayerCsv.NumeroDaCamisa));

                    foreach (var club in clubs)
                    {
                        if (!IsValidChampionship(club.Championship))
                        {
                            continue;
                        }

                        clubCount++;

                        var clubRecord = MapClubToCsv(club);
                        WriteRow(clubsWriter,
                            clubRecord.IdDoClube,
                            clubRecord.Nome,
                            clubRecord.Campeonato,
                            clubRecord.DataDeFundacao,
                            clubRecord.Cidade,
                            clubRecord.Estado,
                            clubRecord.Pais,
                            clubRecord.Estadio,
                            clubRecord.Presidente,
                            clubRecord.Apelido,
                            clubRecord.Cores);

                        if (club.Players == null || club.Players.Count == 0)
                        {
                            continue;
                        }

                        foreach (var player in club.Players)
                        {
                            var playerRecord = MapPlayerToCsv(club.ClubId, player);
                            WriteRow(playersWriter,
                                playerRecord.IdDoClube,
                                playerRecord.IdDoJogador,
                                playerRecord.Nome,
                                playerRecord.Idade,
                                playerRecord.Gols,
                                playerRecord.DataDeEstreia,
                                playerRecord.Posicao,
                                playerRecord.NumeroDaCamisa);
                            playerCount++;
                        }
                    }
                }
            }

            if (clubCount == 0 && playerCount == 0)
            {
                Logger.Warn("Nenhum clube válido encontrado para exportação.");
                return false;
            }

            Logger.Info($"Arquivos gerados em {_outputDir} | clubes: {clubCount} | jogadores: {playerCount}");
            return true;
        }

        private static void WriteRow(StreamWriter writer, params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                writer.WriteLine();
                return;
            }

            var builder = new StringBuilder(values.Length * 16);

            for (var i = 0; i < values.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                builder.Append(EscapeCsvValue(values[i]));
            }

            writer.WriteLine(builder.ToString());
        }

        private static string EscapeCsvValue(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var normalized = value.Replace("\"", "\"\"");
            if (normalized.Contains(',') || normalized.Contains('"') || normalized.Contains('\n') || normalized.Contains('\r'))
            {
                return $"\"{normalized}\"";
            }

            return normalized;
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

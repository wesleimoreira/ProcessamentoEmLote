using ProcessamentoEmLote.Models;
using ProcessamentoEmLote.Utils;
using System.Text.Json;

namespace ProcessamentoEmLote.Services
{
    public class JsonReaderService
    {
        private readonly JsonSerializerOptions _options;

        public JsonReaderService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip, // ignora comentários se houver
                AllowTrailingCommas = true // tolera vírgulas extras
            };
        }

        public IEnumerable<Club> ReadClubs(string jsonFilePath)
        {
            var fullPath = Path.GetFullPath(jsonFilePath);
            var extension = Path.GetExtension(fullPath).ToLowerInvariant();

            if (extension != ".json" && extension != ".jsonl")
            {
                throw new NotSupportedException($"Formato de arquivo não suportado: {extension}. Use apenas .json ou .jsonl.");
            }

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {fullPath}");
            }

            var fileInfo = new FileInfo(fullPath);

            if (fileInfo.Length == 0)
            {
                Logger.Warn("Arquivo vazio, nada a processar.");
                yield break;
            }

            using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
            using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);

            var lineNumber = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                Club? club = null;

                try
                {
                    club = JsonSerializer.Deserialize<Club>(line, _options);
                }
                catch (JsonException ex)
                {
                    Logger.Warn($"Linha {lineNumber} inválida ignorada: {ex.Message}");
                    continue;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Falha inesperada na linha {lineNumber}: {ex.Message}");
                    continue;
                }

                if (club != null)
                {
                    yield return club;
                }
            }
        }
    }
}

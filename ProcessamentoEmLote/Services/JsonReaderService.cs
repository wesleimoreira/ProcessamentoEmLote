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

            // Validação de extensão
            if (extension != ".json" && extension != ".jsonl")
            {
                throw new NotSupportedException($"Formato de arquivo não suportado: {extension}. Use apenas .json ou .jsonl.");
            }

            // Validação de existência
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Arquivo não encontrado: {fullPath}");
            }

            var fileInfo = new FileInfo(fullPath);

            // Validação de arquivo vazio
            if (fileInfo.Length == 0)
            {
                Logger.Warn("Arquivo vazio, nada a processar.");
                yield break;
            }

            int lineNumber = 0;

            foreach (var line in File.ReadLines(fullPath))
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line)) continue;

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

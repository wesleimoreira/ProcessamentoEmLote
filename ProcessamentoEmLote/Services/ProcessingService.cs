using ProcessamentoEmLote.Utils;

namespace ProcessamentoEmLote.Services
{
    public class ProcessingService(JsonReaderService jsonReader, CsvWriterService csvWriter)
    {
        private readonly JsonReaderService _jsonReader = jsonReader ?? throw new ArgumentNullException(nameof(jsonReader));
        private readonly CsvWriterService _csvWriter = csvWriter ?? throw new ArgumentNullException(nameof(csvWriter));

        public void Run(string inputFilePath)
        {
            try
            {
                Logger.Info($"Iniciando processamento do arquivo: {inputFilePath}");

                var clubs = _jsonReader.ReadClubs(inputFilePath);
                var hasData = _csvWriter.WriteCsvFiles(clubs);

                if (!hasData)
                {
                    Logger.Warn("Nenhum clube válido encontrado. Nenhum CSV será gerado.");
                    return;
                }

                Logger.Info("Processamento concluído com sucesso.");
            }
            catch (NotSupportedException ex)
            {
                Logger.Error($"Tipo de arquivo inválido: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Error($"Arquivo não encontrado: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Falha inesperada: {ex.Message}");
            }
        }
    }
}

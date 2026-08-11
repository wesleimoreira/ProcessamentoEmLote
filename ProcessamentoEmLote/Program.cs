using ProcessamentoEmLote.Services;
using ProcessamentoEmLote.Utils;

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath;

        if (args.Length == 0)
        {
            inputFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Input", "sample_clubes.jsonl");

            if (inputFilePath.Length == 0)
            {
                Logger.Warn("[ERROR] Informe o caminho do arquivo JSONL como parâmetro.");
            }
        }
        else
        {
            inputFilePath = args[0];
        }

        var jsonReader = new JsonReaderService();
        var csvWriter = new CsvWriterService();
        var processor = new ProcessingService(jsonReader, csvWriter);

        processor.Run(inputFilePath);
    }
}

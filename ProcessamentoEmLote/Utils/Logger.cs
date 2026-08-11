namespace ProcessamentoEmLote.Utils
{
    public static class Logger
    {
        public static void Info(string message)
        {
            WriteLog("INFO", message, ConsoleColor.Green);
        }

        public static void Warn(string message)
        {
            WriteLog("WARN", message, ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            WriteLog("ERROR", message, ConsoleColor.Red);
        }

        private static void WriteLog(string level, string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");

            Console.ForegroundColor = originalColor;
        }
    }
}

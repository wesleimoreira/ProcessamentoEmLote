namespace ProcessamentoEmLote.Utils
{
    public static class StringUtils
    {
        public static string Safe(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            var needsQuotes = value.Contains(",") || value.Contains("\"") || value.Contains("\n");

            if (needsQuotes)
            {
                var escaped = value.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }

            return value;
        }

        public static string JoinWithPipe(IEnumerable<string>? values)
        {
            if (values == null) return string.Empty;

            return string.Join(" | ", values.Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim()));
        }

        public static string NormalizeUpper(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpperInvariant();
        }

        public static string GetProjectPath(string relativePath)
        {
            var projectRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            return Path.GetFullPath(Path.Combine(projectRoot, relativePath));
        }
    }
}

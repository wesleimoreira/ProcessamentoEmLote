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
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentException("O caminho relativo não pode ser vazio.", nameof(relativePath));
            }

            var baseCandidates = new[]
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };

            foreach (var baseDir in baseCandidates.Distinct())
            {
                var fullPath = Path.GetFullPath(Path.Combine(baseDir, relativePath));
               
                if (File.Exists(fullPath) || Directory.Exists(fullPath))
                {
                    return fullPath;
                }

                var current = new DirectoryInfo(baseDir);

                while (current != null)
                {
                    var candidate = Path.GetFullPath(Path.Combine(current.FullName, relativePath));

                    if (File.Exists(candidate) || Directory.Exists(candidate))
                    {
                        return candidate;
                    }

                    foreach (var childDir in current.GetDirectories())
                    {
                        candidate = Path.GetFullPath(Path.Combine(childDir.FullName, relativePath));

                        if (File.Exists(candidate) || Directory.Exists(candidate))
                        {
                            return candidate;
                        }
                    }

                    current = current.Parent;
                }
            }
           
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", relativePath));
        }
    }
}

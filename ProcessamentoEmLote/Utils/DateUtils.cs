using System.Globalization;

namespace ProcessamentoEmLote.Utils
{
    public static class DateUtils
    {
        public static string ParseDate(string? date)
        {
            if (string.IsNullOrWhiteSpace(date)) return string.Empty;

            if (DateTime.TryParse(date, out var parsed))
            {
                return parsed.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        public static bool IsValidDate(string? date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;

            return DateTime.TryParse(date, out _);
        }

        public static DateTime? ToDateTime(string? date)
        {
            if (string.IsNullOrWhiteSpace(date)) return null;

            if (DateTime.TryParse(date, out var parsed)) return parsed;

            return null;
        }
    }
}

namespace ProcessamentoEmLote.Models
{
    public class Player
    {
        public string PlayerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? Age { get; set; }
        public int? Goals { get; set; }
        public string DebutDate { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int? ShirtNumber { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public decimal? MarketValue { get; set; }
    }
}

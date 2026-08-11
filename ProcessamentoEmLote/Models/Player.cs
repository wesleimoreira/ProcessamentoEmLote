using System.Text.Json.Serialization;

namespace ProcessamentoEmLote.Models
{
    public class Player
    {
        [JsonPropertyName("player_id")]
        public string PlayerId { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("age")]
        public int? Age { get; set; }

        [JsonPropertyName("goals")]
        public int? Goals { get; set; }

        [JsonPropertyName("debut_date")]
        public string DebutDate { get; set; } = default!;

        [JsonPropertyName("position")]
        public string Position { get; set; } = default!;

        [JsonPropertyName("shirt_number")]
        public int? ShirtNumber { get; set; } = default!;
    }
}

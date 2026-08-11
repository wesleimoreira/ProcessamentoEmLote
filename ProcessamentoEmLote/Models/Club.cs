namespace ProcessamentoEmLote.Models
{
    using System.Text.Json.Serialization;

    public class Club
    {
        [JsonPropertyName("club_id")]
        public string ClubId { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("championship")]
        public string Championship { get; set; } = default!;

        [JsonPropertyName("founding_date")]
        public string FoundingDate { get; set; } = default!;

        [JsonPropertyName("city")]
        public string City { get; set; } = default!;

        [JsonPropertyName("state")]
        public string State { get; set; } = default!;

        [JsonPropertyName("country")]
        public string Country { get; set; } = default!;

        [JsonPropertyName("stadium")]
        public string Stadium { get; set; } = default!;

        [JsonPropertyName("president")]
        public string President { get; set; } = default!;

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = default!;

        [JsonPropertyName("colors")]
        public List<string> Colors { get; set; } = default!;

        [JsonPropertyName("players")]
        public List<Player> Players { get; set; } = default!;
    }
}

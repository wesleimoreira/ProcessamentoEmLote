namespace ProcessamentoEmLote.Models
{
    public class Club
    {
        public string ClubId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Championship { get; set; } = string.Empty;
        public string FoundingDate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Stadium { get; set; } = string.Empty;
        public string President { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public List<string>? Colors { get; set; }
        public List<Player>? Players { get; set; }
    }
}

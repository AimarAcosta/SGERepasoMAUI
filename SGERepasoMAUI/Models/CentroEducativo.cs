using System.Text.Json.Serialization;

namespace SGERepasoMAUI.Models
{
    public class CentroEducativo
    {
        [JsonPropertyName("DOMBRE")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("DTITUC")]
        public string Tipo { get; set; } = string.Empty;

        [JsonPropertyName("DTERRE")]
        public string Territorio { get; set; } = string.Empty;

        [JsonPropertyName("DMUNIC")]
        public string Municipio { get; set; } = string.Empty;

        [JsonPropertyName("LATITUD")]
        public double Latitud { get; set; }

        [JsonPropertyName("LONGITUD")]
        public double Longitud { get; set; }
    }
}

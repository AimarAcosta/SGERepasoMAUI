using System.Text.Json.Serialization;

namespace SGERepasoMAUI.Models
{
    // Modelo para la respuesta actual del clima
    public class WeatherResponse
    {
        [JsonPropertyName("main")]
        public MainWeather? Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription>? Weather { get; set; }

        [JsonPropertyName("name")]
        public string? CityName { get; set; }
    }

    public class MainWeather
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
    }

    // Modelo para el pronóstico de varios días
    public class ForecastResponse
    {
        [JsonPropertyName("list")]
        public List<ForecastItem>? List { get; set; }
    }

    public class ForecastItem
    {
        [JsonPropertyName("dt")]
        public long DateTime { get; set; }

        [JsonPropertyName("main")]
        public MainWeather? Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription>? Weather { get; set; }

        [JsonPropertyName("dt_txt")]
        public string? DateTimeText { get; set; }
    }

    // Modelo simplificado para mostrar en la UI
    public class DailyForecast
    {
        public string Dia { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad calculada para el binding en la UI
        public string Resumen => $"{TempMax:F0} / {TempMin:F0}";
    }
}

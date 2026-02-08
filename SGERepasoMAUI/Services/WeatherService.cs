using SGERepasoMAUI.Models;
using System.Text.Json;

namespace SGERepasoMAUI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        
        // IMPORTANTE: Reemplaza con tu API Key de OpenWeatherMap
        // Obtén una gratis en: https://openweathermap.org/api
        private const string API_KEY = "cd42cc3800a62785050c1b93beb038f2";
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Obtiene el clima actual para una ubicación
        /// </summary>
        public async Task<WeatherResponse?> ObtenerClimaActualAsync(double latitud, double longitud)
        {
            try
            {
                var url = $"{BASE_URL}/weather?lat={latitud}&lon={longitud}&appid={API_KEY}&units=metric&lang=es";
                var response = await _httpClient.GetStringAsync(url);
                System.Diagnostics.Debug.WriteLine($"Respuesta clima actual: {response}");
                return JsonSerializer.Deserialize<WeatherResponse>(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo clima: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el pronóstico de los próximos días
        /// </summary>
        public async Task<List<DailyForecast>> ObtenerPronosticoAsync(double latitud, double longitud)
        {
            var pronosticos = new List<DailyForecast>();

            try
            {
                var url = $"{BASE_URL}/forecast?lat={latitud}&lon={longitud}&appid={API_KEY}&units=metric&lang=es";
                var response = await _httpClient.GetStringAsync(url);
                System.Diagnostics.Debug.WriteLine($"Respuesta pronóstico: {response}");
                var forecastResponse = JsonSerializer.Deserialize<ForecastResponse>(response);

                if (forecastResponse?.List != null)
                {
                    // Agrupar por día y obtener uno por día (a las 12:00)
                    var porDia = forecastResponse.List
                        .Where(f => f.DateTimeText != null && f.DateTimeText.Contains("12:00:00"))
                        .Take(5)
                        .ToList();

                    foreach (var item in porDia)
                    {
                        var fecha = DateTimeOffset.FromUnixTimeSeconds(item.DateTime).DateTime;
                        pronosticos.Add(new DailyForecast
                        {
                            Dia = fecha.ToString("ddd dd"),
                            Icono = ObtenerEmoji(item.Weather?.FirstOrDefault()?.Main ?? ""),
                            TempMax = item.Main?.TempMax ?? 0,
                            TempMin = item.Main?.TempMin ?? 0,
                            Descripcion = item.Weather?.FirstOrDefault()?.Description ?? ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo pronóstico: {ex.Message}");
            }

            return pronosticos;
        }

        /// <summary>
        /// Convierte el código del clima a emoji
        /// </summary>
        public string ObtenerEmoji(string weatherMain)
        {
            return weatherMain.ToLower() switch
            {
                "clear" => "☀️",
                "clouds" => "☁️",
                "rain" => "🌧️",
                "drizzle" => "🌦️",
                "thunderstorm" => "⛈️",
                "snow" => "❄️",
                "mist" or "fog" or "haze" => "🌫️",
                _ => "❓"
            };
        }
    }
}

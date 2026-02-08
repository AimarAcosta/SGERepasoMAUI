using SGERepasoMAUI.Models;
using System.Text.Json;
using System.IO;
using Microsoft.Maui.Storage;
using System.Text.Json.Serialization;

namespace SGERepasoMAUI.Services
{
    public class CentrosService
    {
        private class CentrosRoot
        {
            [JsonPropertyName("CENTROS")]
            public List<CentroEducativo> Centros { get; set; } = new();
        }

        public async Task<List<CentroEducativo>> ObtenerCentrosAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("centros.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var root = JsonSerializer.Deserialize<CentrosRoot>(json, options);
                return root?.Centros ?? new List<CentroEducativo>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error leyendo centros.json: {ex}");
                return new List<CentroEducativo>();
            }
        }
    }
}

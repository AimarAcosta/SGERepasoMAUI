using SGERepasoMAUI.Models;
using SGERepasoMAUI.Services;
using System.Globalization;

namespace SGERepasoMAUI.Views
{
    public partial class MapaNativoPage : ContentPage
    {
        private readonly WeatherService _weatherService;
        private readonly CentroEducativo _centro;
        private const string API_KEY = "7668f943897c381e820990e3b9a6aaa9";

        public MapaNativoPage(CentroEducativo centro)
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("DEBUG: MapaNativoPage constructor ejecutado");
            _centro = centro;
            _weatherService = new WeatherService();

            ConfigurarDatosCentro();
            ConfigurarMapa();
            _ = CargarDatosClimaAsync();
        }

        private void ConfigurarDatosCentro()
        {
            LblNombre.Text = _centro.Nombre;
            LblTipo.Text = $"Tipo: {_centro.Tipo}";
            LblMunicipio.Text = $"{_centro.Municipio}, {_centro.Territorio}";
            LblCoordenadas.Text = $"Lat: {_centro.Latitud:F4}, Lon: {_centro.Longitud:F4}";
        }

        private void ConfigurarMapa()
        {
            // Usar Leaflet + OpenStreetMap via WebView (funciona en todas las plataformas)
            var lat = _centro.Latitud.ToString(CultureInfo.InvariantCulture);
            var lon = _centro.Longitud.ToString(CultureInfo.InvariantCulture);
            var nombre = _centro.Nombre.Replace("'", "\\'");
            var municipio = _centro.Municipio.Replace("'", "\\'");

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'/>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <style>
        body {{ margin: 0; padding: 0; }}
        #map {{ width: 100%; height: 100vh; }}
    </style>
</head>
<body>
    <div id='map'></div>
    <script>
        var map = L.map('map').setView([{lat}, {lon}], 15);
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
            attribution: '&copy; OpenStreetMap contributors'
        }}).addTo(map);
        L.marker([{lat}, {lon}])
            .addTo(map)
            .bindPopup('<b>{nombre}</b><br>{municipio}')
            .openPopup();
    </script>
</body>
</html>";

            MapaWebView.Source = new HtmlWebViewSource { Html = html };
        }

        private async Task CargarDatosClimaAsync()
        {
            // Cargar clima actual
            var climaActual = await _weatherService.ObtenerClimaActualAsync(_centro.Latitud, _centro.Longitud);
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadingClima.IsVisible = false;
                LoadingClima.IsRunning = false;

                if (climaActual?.Main != null)
                {
                    var weatherMain = climaActual.Weather?.FirstOrDefault()?.Main ?? "";
                    LblClimaIcono.Text = _weatherService.ObtenerEmoji(weatherMain);
                    LblTemperatura.Text = $"{climaActual.Main.Temperature:F1} C";
                    LblDescripcion.Text = climaActual.Weather?.FirstOrDefault()?.Description ?? "Sin datos";
                    LblHumedad.Text = $"Humedad: {climaActual.Main.Humidity}%";
                }
                else
                {
                    LblClimaIcono.Text = "?";
                    LblTemperatura.Text = "-- C";
                    LblDescripcion.Text = "No disponible (verifica API Key)";
                    LblHumedad.Text = "";
                }
            });

            // Cargar pronostico
            var pronostico = await _weatherService.ObtenerPronosticoAsync(_centro.Latitud, _centro.Longitud);
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadingPronostico.IsVisible = false;
                LoadingPronostico.IsRunning = false;

                if (pronostico.Any())
                {
                    ListaPronostico.ItemsSource = pronostico;
                }
                else
                {
                    ListaPronostico.ItemsSource = new List<DailyForecast>
                    {
                        new() { Dia = "Lun", Icono = "?", TempMax = 20, TempMin = 12, Descripcion = "sin datos" },
                        new() { Dia = "Mar", Icono = "?", TempMax = 22, TempMin = 14, Descripcion = "sin datos" },
                        new() { Dia = "Mie", Icono = "?", TempMax = 18, TempMin = 10, Descripcion = "sin datos" },
                        new() { Dia = "Jue", Icono = "?", TempMax = 19, TempMin = 11, Descripcion = "sin datos" },
                        new() { Dia = "Vie", Icono = "?", TempMax = 21, TempMin = 13, Descripcion = "sin datos" }
                    };
                }
            });
        }
    }
}

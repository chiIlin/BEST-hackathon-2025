// Services/GoogleMapsService.cs
using Microsoft.Extensions.Options;

namespace best_hackathon_2025.Services;

public record LatLng(double Lat, double Lng);

public class GoogleMapsService
{
    private readonly HttpClient _http;
    private readonly string _key;

    public GoogleMapsService(HttpClient http, IOptions<GoogleMapsOptions> opt)
    {
        _http = http;
        _key = opt.Value.ApiKey;
    }

    public async Task<string> GetRouteJsonAsync(LatLng from, LatLng to, string mode = "walking")
    {
        var url = $"https://maps.googleapis.com/maps/api/directions/json" +
                  $"?origin={from.Lat},{from.Lng}&destination={to.Lat},{to.Lng}" +
                  $"&mode={mode}&key={_key}";

        return await _http.GetStringAsync(url);
    }
}

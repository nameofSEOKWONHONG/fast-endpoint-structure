using System.Net.Http.Json;
using Feature.Domain.Tour.Dtos;

namespace Feature.Tour.Tours.OtaProviders;

public class HanaProvider : IOtaProvider
{
    private readonly HttpClient _httpClient;

    public HanaProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<TourSummaryDto> GetTour(CancellationToken cancellationToken)
    {
        var today = DateTime.Now;
        var res = await this._httpClient.GetAsync($"api/tour/{today}", cancellationToken);
        res.EnsureSuccessStatusCode();
        var result = await res.Content.ReadFromJsonAsync<List<Model>>(cancellationToken: cancellationToken);
        var tourSummary = new TourSummaryDto();
        //TODO: Convert summary entity.
        return tourSummary;
    }

    class Model
    {
        public string Nation { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public string HotelId { get; set; }
    }
}
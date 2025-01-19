using Feature.Pdf.Weather;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;

namespace Feature.Pdf;

public static class PdfRegister
{
    public static void AddPdfFeature(this WebApplicationBuilder builder)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        builder.Services.AddScoped<IWeatherPdfService, WeatherPdfService>();
    }
}
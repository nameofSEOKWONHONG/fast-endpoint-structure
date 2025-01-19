using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Feature.Pdf.Weather;

public interface IWeatherPdfService
{
    byte[] GetWeatherPdf(string city);
}

public class WeatherPdfService : ServiceBase<WeatherPdfService>, IWeatherPdfService
{
    /// <summary>
    /// ctor    
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    public WeatherPdfService(ILogger<WeatherPdfService> logger, ISessionContext sessionContext) : base(logger, sessionContext)
    {
    }

    public byte[] GetWeatherPdf(string city)
    {
        return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
        
                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
        
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                
                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });
        
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf();
    }
}
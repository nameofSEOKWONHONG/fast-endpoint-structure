using System.Net.Mime;
using FastEndpoints;
using Feature.Pdf.Weather;

namespace Feature.Pdf;

public class PdfReqeust
{
    public string Type { get; set; }
}

public class PdfEndpoint : Endpoint<PdfReqeust>
{
    private readonly IWeatherPdfService _weatherPdfService;
    public PdfEndpoint(IWeatherPdfService service)
    {
        _weatherPdfService = service;
    }
    
    public override void Configure()
    {
        Get("/api/weatherpdf");
    }
    
    public override async Task HandleAsync(PdfReqeust req, CancellationToken ct)
    {
        var bytes = _weatherPdfService.GetWeatherPdf(string.Empty);
        Stream stream = new MemoryStream(bytes);
        await SendStreamAsync(
            stream: stream,
            fileName: "weatherpdf.pdf",
            fileLengthBytes: stream.Length,
            contentType: MediaTypeNames.Application.Pdf, cancellation: ct);
    }
}
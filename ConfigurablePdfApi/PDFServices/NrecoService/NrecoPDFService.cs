using ConfigurablePdfApi.PDFServices.Interfaces;
using NReco.PdfGenerator;

namespace ConfigurablePdfApi.PDFServices.NrecoService;

public class NrecoPDFService : IPDFService
{
    private readonly HtmlToPdfConverter _converter;

    public NrecoPDFService(IConfiguration configuration)
    {
        var licenseKey = Environment.GetEnvironmentVariable("NRECO_PDF_LICENSE_KEY");

        _converter = new HtmlToPdfConverter();

        if (!string.IsNullOrWhiteSpace(licenseKey))
            _converter.License.SetLicenseKey("DEMO", licenseKey);
        else
            throw new Exception("No licence key");
    }

    public Task<byte[]> GeneratePDF(string html)
    {
        var pdfBytes = _converter.GeneratePdf(html);
        return Task.FromResult(pdfBytes);
    }
}
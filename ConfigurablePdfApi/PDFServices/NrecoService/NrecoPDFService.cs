using ConfigurablePdfApi.PDFServices.Interfaces;
using NReco.PdfGenerator;

namespace ConfigurablePdfApi.PDFServices.NrecoService;

public class NrecoPDFService : IPDFService
{
    private readonly string _licenseKey;

    public NrecoPDFService()
    {
        var licenseKey = Environment.GetEnvironmentVariable("NRECO_PDF_LICENSE_KEY");

        if (!string.IsNullOrWhiteSpace(licenseKey))
            _licenseKey = licenseKey;
        else
            throw new Exception("No licence key");
    }

    public Task<byte[]> GeneratePDF(string html)
    {
        var converter = new HtmlToPdfConverter();
        converter.License.SetLicenseKey("DEMO", _licenseKey);
        var pdfBytes = converter.GeneratePdf(html);
        return Task.FromResult(pdfBytes);
    }
}
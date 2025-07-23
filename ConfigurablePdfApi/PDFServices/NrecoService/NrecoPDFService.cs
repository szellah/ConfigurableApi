using ConfigurablePdfApi.PDFServices.Interfaces;

namespace ConfigurablePdfApi.PDFServices.NrecoService;

public class NrecoPDFService : IPDFService
{
    public Task<byte[]> GeneratePDF(string html)
    {
        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        var pdfBytes = htmlToPdf.GeneratePdf(html);

        return Task.FromResult(pdfBytes);
    }
}
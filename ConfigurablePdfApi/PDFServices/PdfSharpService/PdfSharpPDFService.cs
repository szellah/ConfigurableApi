using System.Text;
using ConfigurablePdfApi.PDFServices.Interfaces;
using PdfSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ConfigurablePdfApi.PDFServices.PdfSharpService;

public class PdfSharpPDFService : IPDFService
{
    public async Task<byte[]> GeneratePDF(string html)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
        using var ms = new MemoryStream();
        pdf.Save(ms);
        pdf.Close();

        return ms.ToArray();
    }
}
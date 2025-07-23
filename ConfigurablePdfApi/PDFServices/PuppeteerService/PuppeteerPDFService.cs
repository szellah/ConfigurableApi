using ConfigurablePdfApi.PDFServices.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace ConfigurablePdfApi.PDFServices.PuppeteerService;

public class PuppeteerPdfService : IPDFService
{
    public async Task<byte[]> GeneratePDF(string html)
    {
        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        
        var pdfBytes = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4
        });
        
        return pdfBytes; 
    }
}
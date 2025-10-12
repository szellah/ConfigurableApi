using ConfigurablePdfApi.PDFServices.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace ConfigurablePdfApi.PDFServices.PuppeteerService;

public class PuppeteerPdfService : IPDFService
{
    private static IBrowser? _browser;
    public async Task<byte[]> GeneratePDF(string html)
    {
        if (_browser is null)
            await InitPuppeteerAsync();

        if (_browser is null) throw new Exception();
        
        var page = await _browser.NewPageAsync();
        await page.SetContentAsync(html);
        
        var pdfBytes = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4
        });
        
        await page.CloseAsync();
        
        return pdfBytes; 
    }
    
    static async Task InitPuppeteerAsync()
    {
        await new BrowserFetcher().DownloadAsync();
        _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    }
}
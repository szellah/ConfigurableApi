using ConfigurablePdfApi.PDFServices.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Playwright;

namespace ConfigurablePdfApi.PDFServices.PlaywrightService;

public class PlaywrightPDFService(
    IServiceProvider serviceProvider, 
    ILoggerFactory loggerFactory) : IPDFService
{
    
    public async Task<byte[]> GeneratePDF(string html)
    {
        Microsoft.Playwright.Program.Main(["install"]);
        // return;

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
        
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        var pdfBytes =await page.PdfAsync(new PagePdfOptions
        {
            Format = "A4"
        });
        
        await page.CloseAsync();
        return pdfBytes; 
    }
}
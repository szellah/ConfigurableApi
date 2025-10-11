using ConfigurablePdfApi.PDFServices.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Playwright;

namespace ConfigurablePdfApi.PDFServices.PlaywrightService;

public class PlaywrightPDFService : IPDFService
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    public async Task<byte[]> GeneratePDF(string html)
    {
        //Microsoft.Playwright.Program.Main(["install"]);
        // return;

        if (_browser is null)
            await InitPlaywrightAsync();

        if (_browser is null) return [];
        
        var page = await _browser.NewPageAsync();
        await page.SetContentAsync(html);

        var pdfBytes = await page.PdfAsync(new PagePdfOptions
        {
            Format = "A4"
        });
        
        await page.CloseAsync();
        return pdfBytes;

    }
    
    static async Task InitPlaywrightAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }
}
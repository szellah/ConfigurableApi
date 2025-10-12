using ConfigurablePdfApi.PDFServices.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Threading;

namespace ConfigurablePdfApi.PDFServices.PuppeteerService;

public class PuppeteerPdfService : IPDFService, IAsyncDisposable
{
    private static IBrowser? _browser;
    private static readonly SemaphoreSlim _semaphore = new(5); // limit concurrent renders

    public PuppeteerPdfService()
    {
        _browser = Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true, Args = ["--no-sandbox", "--disable-setuid-sandbox"]
        }).GetAwaiter().GetResult();   
    }

    public async Task<byte[]> GeneratePDF(string html)
    {
        if (_browser == null)
            throw new InvalidOperationException("Browser not initialized");

        await _semaphore.WaitAsync(); // prevent overload
        try
        {
            await using var page = await _browser.NewPageAsync();
            await page.SetContentAsync(html);

            return await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4
            });
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null)
            await _browser.CloseAsync();
    }
}
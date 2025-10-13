using System.Text;
using ConfigurablePdfApi.PDFServices.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConfigurablePdfApi.PDFServices.SeleniumService;

public class SeleniumPDFService : IPDFService, IDisposable
{
    private readonly ChromeDriver _driver;
    private readonly SemaphoreSlim _semaphore = new(5);

    public SeleniumPDFService()
    {
        _driver = InitializeChromeDriver();
    }

    private ChromeDriver InitializeChromeDriver()
    {
        var options = new ChromeOptions();

        // Headless mode & stability options
        options.AddArgument("--headless=new");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        
        return new ChromeDriver(options);
    }

    public async Task<byte[]> GeneratePDF(string html)
    {
        await _semaphore.WaitAsync();
        try
        {
            // Create a data URL to render HTML
            var htmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
            _driver.Url = $"data:text/html;base64,{htmlBase64}";

            var printOptions = new PrintOptions
            {
                PageDimensions = PrintOptions.PageSize.A4,
            };

            var file = _driver.Print(printOptions);
            return file.AsByteArray;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
using System.Text;
using ConfigurablePdfApi.PDFServices.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConfigurablePdfApi.PDFServices.SeleniumService;

public class SeleniumPDFService : IPDFService
{
    public Task<byte[]> GeneratePDF(string html)
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        using var driver = new ChromeDriver(options);
        var htmlBs64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
        driver.Url = $"data:text/html;base64,{htmlBs64}";
        
        var file = driver.Print(new PrintOptions
        {
            PageDimensions = PrintOptions.PageSize.A4,
       });
        
        driver.Quit();
         
        return Task.FromResult(file.AsByteArray); 
    }
}
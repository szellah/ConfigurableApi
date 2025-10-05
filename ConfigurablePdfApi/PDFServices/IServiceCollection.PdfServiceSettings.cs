using CommandLine;
using ConfigurablePdfApi.PDFServices.CefService;
using ConfigurablePdfApi.PDFServices.Interfaces;
using ConfigurablePdfApi.PDFServices.NrecoService;
using ConfigurablePdfApi.PDFServices.PdfSharpService;
using ConfigurablePdfApi.PDFServices.PlaywrightService;
using ConfigurablePdfApi.PDFServices.PuppeteerService;
using ConfigurablePdfApi.PDFServices.SeleniumService;

namespace ConfigurablePdfApi.PDFServices;

public static partial class IServiceCollection_PdfServiceSettings
{
    public static void AddPdfService(this IServiceCollection services, ParserResult<PdfServicesOptions>? options)
    {
        options.WithParsed(o =>
        {
            Console.WriteLine($"Now using {o.Pdf}");
            switch (o.Pdf)
            {
                case PdfImplementation.Cef:
                    services.AddSingleton<IPDFService, CefPDFService>(); break;
                case PdfImplementation.PdfSharp:
                    services.AddSingleton<IPDFService, PdfSharpPDFService>(); break;
                case PdfImplementation.Puppeteer:
                    services.AddSingleton<IPDFService, PuppeteerPdfService>(); break;
                case PdfImplementation.Selenium:
                    services.AddSingleton<IPDFService, SeleniumPDFService>(); break;
                case PdfImplementation.Playwright:
                    services.AddSingleton<IPDFService, PlaywrightPDFService>(); break;
                case PdfImplementation.Nreco:
                default:
                    services.AddSingleton<IPDFService, NrecoPDFService>(); break;
            }
        });
    }
}
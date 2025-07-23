using CommandLine;

namespace ConfigurablePdfApi.PDFServices;

public enum PdfImplementation
{
    Nreco,
    Cef,
    PdfSharp,
    Puppeteer,
    Selenium,
    Playwright
}

public class PdfServicesOptions
{
    [Option("pdf", Required = false, HelpText = "PDF implementation to use (Nreco, Cef, PdfSharp, Puppeteer, Selenium, Playwright).")]
    public PdfImplementation Pdf { get; set; }
}
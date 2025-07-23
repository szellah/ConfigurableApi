using CefSharp;
using CefSharp.OffScreen;
using ConfigurablePdfApi.PDFServices.Interfaces;

namespace ConfigurablePdfApi.PDFServices.CefService;

public class CefPDFService : IPDFService
{
    public async Task<byte[]> GeneratePDF(string html)
    {
        var browser = new ChromiumWebBrowser();
        
        var tcs = new TaskCompletionSource<bool>();
        
        EventHandler<LoadingStateChangedEventArgs>? handler = null;
        handler = (_, args) =>
        {
            if (args.IsLoading) return;
            browser.LoadingStateChanged -= handler;
            tcs.TrySetResult(true);
        };

        browser.LoadingStateChanged += handler;
        
        browser.LoadHtml(html);

        await tcs.Task;

        var filePath = $"{Path.GetTempPath()}{Guid.NewGuid().ToString()}.pdf";
        
        await browser.PrintToPdfAsync(filePath);

        var pdfBytes = await File.ReadAllBytesAsync(filePath);
        File.Delete(filePath);
        
        return pdfBytes; 
    }
}
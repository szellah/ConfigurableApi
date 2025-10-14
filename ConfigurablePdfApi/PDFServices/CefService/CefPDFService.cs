using CefSharp;
using CefSharp.OffScreen;
using ConfigurablePdfApi.PDFServices.Interfaces;

namespace ConfigurablePdfApi.PDFServices.CefService;

public class CefPDFService : IPDFService, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(5); // Limit concurrent prints
    private bool _initialized;

    public CefPDFService()
    {
        InitializeCef();
    }

    private void InitializeCef()
    {
        if (_initialized)
            return;

        var settings = new CefSettings
        {
            WindowlessRenderingEnabled = true,
            LogSeverity = LogSeverity.Disable
        };

        if (!(Cef.IsInitialized ?? false))
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

        _initialized = true;
    }

    public async Task<byte[]> GeneratePDF(string html)
    {
        await _semaphore.WaitAsync();
        try
        {
            using var browser = new ChromiumWebBrowser();

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<LoadingStateChangedEventArgs>? handler = null;
            handler = (_, args) =>
            {
                if (args.IsLoading) return;
                browser.LoadingStateChanged -= handler;
                tcs.TrySetResult(true);
            };

            browser.LoadingStateChanged += handler;
            browser.LoadHtml(html);

            await tcs.Task.ConfigureAwait(false);

            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
            await browser.PrintToPdfAsync(filePath);

            var pdfBytes = await File.ReadAllBytesAsync(filePath);
            File.Delete(filePath);

            return pdfBytes;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        if (Cef.IsInitialized ?? false)
            Cef.Shutdown();
    }
}
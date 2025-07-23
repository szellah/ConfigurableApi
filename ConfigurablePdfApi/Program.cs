using CommandLine;
using ConfigurablePdfApi.PDFServices;
using ConfigurablePdfApi.PDFServices.Interfaces;
using PdfApi.Models;

namespace ConfigurablePdfApi;

public class Program
{
    public static void Main(string[] args)
    {
        var options = Parser.Default.ParseArguments<PdfServicesOptions>(args);
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddPdfService(options);

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // map get pdf file from html

        app.MapPost("/generate", (PdfGenerateModel model, IPDFService pdfService) =>
        {
            Console.WriteLine(model.Html);
            
            // TODO: Validation?
            
            var file = pdfService.GeneratePDF(model.Html);
            return file;
        });

        app.Run();
    }
}
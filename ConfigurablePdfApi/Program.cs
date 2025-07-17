using PdfApi.Models;

namespace ConfigurablePdfApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddOpenApi();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // map get pdf file from html

        app.MapPost("/generate", (PdfGenerateModel model) =>
        {
            Console.WriteLine(model.Html);
            var file = File.ReadAllBytes("Resources/sample_pdf.pdf");
            return file;
        });

        app.Run();
    }
}
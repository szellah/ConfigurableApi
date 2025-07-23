namespace ConfigurablePdfApi.PDFServices.Interfaces;

public interface IPDFService
{
    public Task<byte[]> GeneratePDF(string html);
}
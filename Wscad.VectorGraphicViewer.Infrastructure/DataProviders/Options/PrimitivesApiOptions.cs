namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Options;

public class PrimitivesApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string PrimitivesEndpoint { get; set; } = "/api/primitives";
    public string ApiKey { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetries { get; set; } = 3;
}
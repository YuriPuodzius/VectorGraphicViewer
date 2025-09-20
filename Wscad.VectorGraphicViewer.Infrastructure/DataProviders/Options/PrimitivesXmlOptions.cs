namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Options;

public class PrimitivesXmlOptions
{
    public string PrimitivesPath { get; set; } = string.Empty;
    public string? RootElement { get; set; }
    public string? Namespace { get; set; }
    public bool ValidateSchema { get; set; }
    public string? SchemaPath { get; set; }
}
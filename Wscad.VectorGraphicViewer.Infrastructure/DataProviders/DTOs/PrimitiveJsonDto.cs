namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;

/// <summary>
/// DTO that mirrors the JSON payload.
/// Keeps everything as strings/primitives to stay format-agnostic.
/// </summary>
internal sealed class PrimitiveJsonDto
{
    public string? type { get; set; }

    // Coordinates as text (e.g., "10; 0")
    public string? a { get; set; }
    public string? b { get; set; }
    public string? c { get; set; }
    public string? center { get; set; }

    // Optional properties
    public double? radius { get; set; }
    public bool? filled { get; set; }

    // Color as text (e.g., "127; 255; 0; 255")
    public string? color { get; set; }
}
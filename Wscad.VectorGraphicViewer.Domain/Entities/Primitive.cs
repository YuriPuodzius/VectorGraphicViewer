namespace Wscad.VectorGraphicViewer.Domain.Entities;

using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Extensions;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public class Primitive
{
    public string Type { get; set; } = string.Empty;

    // Generic coordinates
    public Point? A { get; set; }
    public Point? B { get; set; }
    public Point? C { get; set; }
    public Point? Center { get; set; }

    // Optional properties
    public double? Radius { get; set; }
    public bool? Filled { get; set; }
    public Rgba? Color { get; set; }
    public bool IsActive { get; set; } = true;

    public PrimitiveTypeEnum Kind => PrimitiveTypeExtensions.ParseOrUnknown(Type);
}
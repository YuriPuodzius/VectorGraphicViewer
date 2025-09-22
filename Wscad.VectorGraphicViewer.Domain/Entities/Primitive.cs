using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Extensions;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

namespace Wscad.VectorGraphicViewer.Domain.Entities;

public class Primitive
{
    public string Type { get; set; } = string.Empty;

    // Coordenadas genéricas
    public Point? A { get; set; }
    public Point? B { get; set; }
    public Point? C { get; set; }
    public Point? Center { get; set; }

    // Propriedades opcionais
    public double? Radius { get; set; }
    public bool? Filled { get; set; }
    public Rgba? Color { get; set; }   // torna opcional
    public bool IsActive { get; set; } = true;

    public PrimitiveTypeEnum Kind => PrimitiveTypeExtensions.ParseOrUnknown(Type);
}
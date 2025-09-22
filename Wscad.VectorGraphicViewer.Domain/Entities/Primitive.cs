using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Extensions;

namespace Wscad.VectorGraphicViewer.Domain.Entities;

public class Primitive
{
    public string Type { get; set; } = string.Empty;

    // Coordenadas genéricas
    public ValueObjects.Point? A { get; set; }
    public ValueObjects.Point? B { get; set; }
    public ValueObjects.Point? C { get; set; }
    public ValueObjects.Point? Center { get; set; }

    // Propriedades opcionais
    public double? Radius { get; set; }
    public bool? Filled { get; set; }
    public ValueObjects.Rgba? Color { get; set; }   // torna opcional
    public bool IsActive { get; set; } = true;

    public PrimitiveTypeEnum Kind => PrimitiveTypeExtensions.ParseOrUnknown(Type);
}
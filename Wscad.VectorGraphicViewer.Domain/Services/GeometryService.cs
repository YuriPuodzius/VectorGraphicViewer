using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

namespace Wscad.VectorGraphicViewer.Domain.Services;

public class GeometryService : IGeometryService
{
    public Rgba GetColor(Primitive p) => p.Color ?? new Rgba(0, 0, 0, 255); // default: preto opaco

    public (Point? A, Point? B, Point? C, Point? Center) GetPoints(Primitive p) => (p.A, p.B, p.C, p.Center);
}
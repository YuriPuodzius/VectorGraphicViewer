using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

namespace Wscad.VectorGraphicViewer.Domain.Services;

public class GeometryService : IGeometryService
{
    public IReadOnlyList<PrimitiveTypeEnum> AvailablePrimitivesRules(IReadOnlyList<Primitive> primitives)
    {
        IReadOnlyList<PrimitiveTypeEnum> primitiveTypeEnums = primitives.Where(p => p.IsActive)
          .Select(p => p.Kind)
          .Distinct()
          .OrderBy(k => k)
          .ToList();

        return primitiveTypeEnums;
    }

    public Rgba GetColor(Primitive p) => p.Color ?? new Rgba(0, 0, 0, 255); // default: preto opaco

    public (Point? A, Point? B, Point? C, Point? Center) GetPoints(Primitive p) => (p.A, p.B, p.C, p.Center);
}
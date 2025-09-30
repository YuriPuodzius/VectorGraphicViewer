using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

namespace Wscad.VectorGraphicViewer.Domain.Services;

/// <summary>
/// GeometryService centralizes the domain business rules,
/// i.e., the logic that defines which primitives are available,
/// how colors and points are handled, etc.
///
/// Unlike the *Drawers* (in the WPF/Infrastructure layer),
/// this service does not deal with visual rendering.
/// *Drawers* are only responsible for translating a Primitive
/// into a drawing on the screen (UI), while this service
/// contains pure business rules, independent of the UI.
///
/// In short:
/// - Domain.Services (e.g., GeometryService): pure business rules,
///   independent of any UI technology.
/// - Infrastructure.Drawing.Drawers: presentation rules,
///   specific to rendering a Primitive in WPF.
/// </summary>

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
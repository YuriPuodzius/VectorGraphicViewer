namespace Wscad.VectorGraphicViewer.Domain.Services;

using System.Collections.Generic;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public interface IGeometryService
{
    IReadOnlyList<PrimitiveTypeEnum> AvailablePrimitivesRules(IReadOnlyList<Primitive> primitives);
    Rgba GetColor(Primitive p);
    (Point? A, Point? B, Point? C, Point? Center) GetPoints(Primitive p);
}

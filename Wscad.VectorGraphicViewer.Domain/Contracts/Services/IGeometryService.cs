namespace Wscad.VectorGraphicViewer.Domain.Services;

using System.Collections.Generic;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public interface IGeometryService
{
    IReadOnlyList<PrimitiveTypeEnum> AvailablePrimitivesRules(IReadOnlyList<Primitive> primitives);

    // Normaliza campos úteis para render (ex.: parse de cor/pontos quando necessário).
    // Mantemos simples por enquanto.
    Rgba GetColor(Primitive p);
    // Retorna (A,B,C,Center) já como Point (quando existirem).
    (Point? A, Point? B, Point? C, Point? Center) GetPoints(Primitive p);
}

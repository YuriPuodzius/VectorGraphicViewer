namespace Wscad.VectorGraphicViewer.Domain.Services;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public interface IGeometryService
{
    // Normaliza campos úteis para render (ex.: parse de cor/pontos quando necessário).
    // Mantemos simples por enquanto.
    Rgba GetColor(Primitive p);
    // Retorna (A,B,C,Center) já como Point (quando existirem).
    (Point? A, Point? B, Point? C, Point? Center) GetPoints(Primitive p);
}

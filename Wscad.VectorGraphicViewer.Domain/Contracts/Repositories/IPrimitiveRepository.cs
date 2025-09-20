namespace Wscad.VectorGraphicViewer.Domain.Contracts;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public interface IPrimitiveRepository
{
    IReadOnlyList<Primitive> GetAll();
    Primitive? GetByType(PrimitiveTypeEnum type);
}
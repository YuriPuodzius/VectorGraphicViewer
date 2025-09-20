namespace Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public interface IPrimitivesDataSource
{
    IReadOnlyList<Primitive> GetAll();
    Primitive? GetByType(PrimitiveTypeEnum type);
}
namespace Wscad.VectorGraphicViewer.Infrastructure.Contracts.Repositories;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public interface IPrimitivesDataSource
{
    IReadOnlyList<Primitive> GetAll();
    IReadOnlyList<Primitive> GetByType(PrimitiveTypeEnum type);
}
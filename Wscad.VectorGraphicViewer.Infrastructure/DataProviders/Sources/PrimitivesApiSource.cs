namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Sources;

using System.Linq;
using Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public class PrimitivesApiSource : IPrimitivesDataSource
{
    public IReadOnlyList<Primitive> GetAll() => Array.Empty<Primitive>();

    public Primitive? GetByType(PrimitiveTypeEnum type) => GetAll().FirstOrDefault(p => p.Kind == type);
}
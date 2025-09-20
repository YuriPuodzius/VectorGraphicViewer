namespace Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public interface IPrimitiveService
{
    IReadOnlyList<Primitive> GetAll();
    Primitive? GetByType(PrimitiveTypeEnum type);
}
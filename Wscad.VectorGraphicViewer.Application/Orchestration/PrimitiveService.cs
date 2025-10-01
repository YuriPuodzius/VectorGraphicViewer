namespace Wscad.VectorGraphicViewer.Application.Orchestration;

using System.Collections.Generic;
using Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;
using Wscad.VectorGraphicViewer.Domain.Contracts;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Services;

/// <summary>
/// PrimitiveService acts as an orchestration layer,
/// delegating calls to repositories and domain services,
/// and preparing the results to be returned to the application.
/// 
/// It serves as the bridge between the presentation layer
/// and the domain, ensuring decoupling and preserving
/// business rules. This is also the right place for simple
/// adaptations, such as mapping entities into Models
/// tailored for the UI or external interfaces.
/// </summary>

public sealed class PrimitiveService : IPrimitiveService
{
    private readonly IPrimitiveRepository _repo;
    private readonly IGeometryService _geometry;

    public PrimitiveService(IPrimitiveRepository repo, IGeometryService geometry)
    {
        _repo = repo;
        _geometry = geometry;
    }

    public IReadOnlyList<Primitive> GetAll()
    {
        IReadOnlyList<Primitive> primitiveList = _repo.GetAll();

        _ = primitiveList.Select(p => _geometry.GetColor(p)).ToList();
        return primitiveList;
    }

    public Primitive? GetByType(PrimitiveTypeEnum type)
    {
        Primitive? primitiveTyped = _repo.GetByType(type);

        _ = _geometry.GetColor(primitiveTyped);

        return primitiveTyped;
    }

    public IReadOnlyList<PrimitiveTypeEnum> GetAvailablePrimitives()
    {
        IReadOnlyList<Primitive> primitives = _repo.GetAll();
        IReadOnlyList<PrimitiveTypeEnum> availablePrimitives = _geometry.AvailablePrimitivesRules(primitives);
        
        return availablePrimitives;
    }
}
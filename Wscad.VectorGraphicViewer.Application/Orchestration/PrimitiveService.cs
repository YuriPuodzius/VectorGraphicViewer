namespace Wscad.VectorGraphicViewer.Application.Orchestration;

using System.Collections.Generic;
using Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;
using Wscad.VectorGraphicViewer.Domain.Contracts;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Services;

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

        // Right place for simple policies (e.g., lazy normalization, logging, etc.)

        // Example: adjust color to ensure default alpha (without modifying the entity itself)
        _ = primitiveList.Select(p => _geometry.GetColor(p)).ToList();
        return primitiveList;
    }

    public Primitive? GetByType(PrimitiveTypeEnum type)
    {
        Primitive? primitiveTyped = _repo.GetByType(type);

        // Right place for simple policies (e.g., lazy normalization, logging, etc.)

        // Example: adjust color to ensure default alpha (without modifying the entity itself)

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
namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Sources;

using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Options;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Mappers;

public class PrimitivesXmlSource : IPrimitivesDataSource
{
    private readonly PrimitivesXmlOptions _opt;

    public PrimitivesXmlSource(IOptions<PrimitivesXmlOptions> options)
    {
        _opt = options.Value;
    }

    private static XmlSerializer CreateSerializer(string? rootName, string? ns)
    {
        // Default root = "primitives" (same as workload file)
        XmlRootAttribute root = new XmlRootAttribute(rootName ?? "primitives")
        {
            Namespace = string.IsNullOrWhiteSpace(ns) ? null : ns
        };

        return new XmlSerializer(typeof(PrimitivesEnvelopeDto), root);
    }

    private IReadOnlyList<Primitive> ReadAll()
    {
        XmlSerializer serializer = CreateSerializer(_opt.RootElement, _opt.Namespace);

        string fullPath = Path.Combine(AppContext.BaseDirectory, _opt.PrimitivesPath);
        using StreamReader sr = new StreamReader(fullPath);

        PrimitivesEnvelopeDto envelope = (PrimitivesEnvelopeDto)serializer.Deserialize(sr)!;

        List<PrimitiveXmlDto> dtos = envelope.Items ?? new List<PrimitiveXmlDto>();
        List<Primitive> list = dtos.Select(dto => dto.ToDomain()).ToList();

        return list;
    }

    public IReadOnlyList<Primitive> GetAll()
    {
        return ReadAll();
    }

    public Primitive? GetByType(PrimitiveTypeEnum type)
    {
        return ReadAll().FirstOrDefault(p => p.Kind == type);
    }
}

// ===== DTOs used only in Infrastructure =====

/// <summary>
/// Envelope DTO for XML:
/// <primitives>
///   <primitive>...</primitive>
///   <primitive>...</primitive>
/// </primitives>
/// </summary>
[XmlType("primitives")]
public sealed class PrimitivesEnvelopeDto
{
    [XmlElement("primitive")]
    public List<PrimitiveXmlDto>? Items { get; set; }
}

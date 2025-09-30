namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Mappers;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;
using Wscad.VectorGraphicViewer.Domain.Extensions;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;

/// <summary>
/// Maps XML DTOs to the domain Primitive.
/// </summary>
internal static class PrimitiveXmlMapper
{
    public static Primitive ToDomain(this PrimitiveXmlDto dto)
    {
        Primitive primitive = new Primitive
        {
            Type = dto.Type ?? string.Empty,
            A = string.IsNullOrWhiteSpace(dto.A) ? null : PointExtensions.Parse(dto.A),
            B = string.IsNullOrWhiteSpace(dto.B) ? null : PointExtensions.Parse(dto.B),
            C = string.IsNullOrWhiteSpace(dto.C) ? null : PointExtensions.Parse(dto.C),
            Center = string.IsNullOrWhiteSpace(dto.Center) ? null : PointExtensions.Parse(dto.Center),
            Radius = dto.Radius,
            Filled = dto.Filled ?? false,
            Color = string.IsNullOrWhiteSpace(dto.Color) ? new Rgba(0, 0, 0, 255) : RgbaExtensions.Parse(dto.Color),
            IsActive = dto.IsActive ?? true
        };

        return primitive;
    }

    public static List<Primitive> ToDomain(this IEnumerable<PrimitiveXmlDto> items)
        => items.Select(ToDomain).ToList();
}
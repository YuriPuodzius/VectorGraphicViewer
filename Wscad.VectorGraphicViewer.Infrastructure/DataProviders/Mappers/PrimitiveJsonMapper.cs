namespace Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Mappers;

using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;
using Wscad.VectorGraphicViewer.Domain.Extensions;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Dtos;


/// <summary>
/// Maps JSON DTOs to the domain Primitive.
/// </summary>
internal static class PrimitiveJsonMapper
{
    public static Primitive ToDomain(this PrimitiveJsonDto dto)
    {
        Primitive primitive = new Primitive
        {
            Type = dto.type ?? string.Empty,
            A = string.IsNullOrWhiteSpace(dto.a) ? null : PointExtensions.Parse(dto.a),
            B = string.IsNullOrWhiteSpace(dto.b) ? null : PointExtensions.Parse(dto.b),
            C = string.IsNullOrWhiteSpace(dto.c) ? null : PointExtensions.Parse(dto.c),
            Center = string.IsNullOrWhiteSpace(dto.center) ? null : PointExtensions.Parse(dto.center),
            Radius = dto.radius,
            Filled = dto.filled ?? false,
            Color = string.IsNullOrWhiteSpace(dto.color) ? new Rgba(0, 0, 0, 255) : RgbaExtensions.Parse(dto.color),
            IsActive = dto.isActive ?? true
        };

        return primitive;
    }

    public static List<Primitive> ToDomain(this IEnumerable<PrimitiveJsonDto> items) => items.Select(ToDomain).ToList();
}
namespace Wscad.VectorGraphicViewer.Domain.Extensions;

using Wscad.VectorGraphicViewer.Domain.Enums;


public static class PrimitiveTypeExtensions
{
    public static PrimitiveTypeEnum ParseOrUnknown(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return PrimitiveTypeEnum.Unknown;

        string t = type.Trim().Replace("_", "").Replace("-", "").ToLowerInvariant();

        switch (t)
        {
            case "line": return PrimitiveTypeEnum.Line;
            case "circle": return PrimitiveTypeEnum.Circle;
            case "triangle": return PrimitiveTypeEnum.Triangle;
            //case "rectangle": return PrimitiveTypeEnum.Rectangle;

            default: return PrimitiveTypeEnum.Unknown;
        }
    }
}
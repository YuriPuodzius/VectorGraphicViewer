using Wscad.VectorGraphicViewer.Domain.Enums;

namespace Wscad.VectorGraphicViewer.Domain.Extensions
{
    public static class PrimitiveTypeExtensions
    {
        /// <summary>
        /// Converte a string do JSON para ShapeType, de forma tolerante.
        /// Retorna Unknown se não reconhecer (extensível sem quebrar).
        /// </summary>
        public static PrimitiveTypeEnum ParseOrUnknown(string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return PrimitiveTypeEnum.Unknown;

            // normaliza: "Line", "line", "LINE" => line
            string t = type.Trim().Replace("_", "").Replace("-", "").ToLowerInvariant();

            return t switch
            {
                "line" => PrimitiveTypeEnum.Line,
                "circle" => PrimitiveTypeEnum.Circle,
                "triangle" => PrimitiveTypeEnum.Triangle,
                "rectangle" => PrimitiveTypeEnum.Rectangle,
                "polygon" => PrimitiveTypeEnum.Polygon,
                _ => PrimitiveTypeEnum.Unknown
            };
        }
    }
}
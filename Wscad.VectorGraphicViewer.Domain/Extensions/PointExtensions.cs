using System.Globalization;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

namespace Wscad.VectorGraphicViewer.Domain.Extensions
{
    public static class PointExtensions
    {
        public static Point Parse(string text)
        {
            string[] parts = text.Split(';');
            return new Point(
                double.Parse(parts[0], CultureInfo.InvariantCulture),
                double.Parse(parts[1], CultureInfo.InvariantCulture)
            );
        }

        public static bool TryParse(string text, out Point result)
        {
            // Usa direto o TryParse nativo do double
            string[] parts = text.Split(';');

            double x = 0, y = 0;
            bool ok = double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x)
                   && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y);

            result = ok ? new Point(x, y) : default;
            return ok;
        }
    }
}
namespace Wscad.VectorGraphicViewer.Domain.Extensions;

using System.Globalization;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public static class PointExtensions
{
    public static Point Parse(string text)
    {
        if (!TryParse(text, out var p))
            throw new FormatException($"Invalid point: '{text}'");

        return p;
    }

    public static bool TryParse(string text, out Point result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        var parts = text.Split(';');
        if (parts.Length < 2)
            return false;

        var sx = parts[0].Trim().Replace(',', '.');
        var sy = parts[1].Trim().Replace(',', '.');

        double x, y;
        bool okX = double.TryParse(sx, NumberStyles.Float, CultureInfo.InvariantCulture, out x);
        bool okY = double.TryParse(sy, NumberStyles.Float, CultureInfo.InvariantCulture, out y);

        if (!(okX && okY))
            return false;

        result = new Point(x, y);
        return true;
    }
}
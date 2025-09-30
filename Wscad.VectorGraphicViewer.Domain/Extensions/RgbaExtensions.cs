namespace Wscad.VectorGraphicViewer.Domain.Extensions;

using System.Globalization;
using Wscad.VectorGraphicViewer.Domain.ValueObjects;

public static class RgbaExtensions
{
    public static Rgba Parse(string text)
    {
        string[] parts = text.Split(';');
        byte r = byte.Parse(parts[0], CultureInfo.InvariantCulture);
        byte g = byte.Parse(parts[1], CultureInfo.InvariantCulture);
        byte b = byte.Parse(parts[2], CultureInfo.InvariantCulture);
        byte a = parts.Length > 3 ? byte.Parse(parts[3], CultureInfo.InvariantCulture) : (byte)255;

        return new Rgba(r, g, b, a);
    }

    public static bool TryParse(string text, out Rgba result)
    {
        string[] parts = text.Split(';');

        byte r = 0, g = 0, b = 0, a = 255; // default inicialization
        bool ok = byte.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out r)
               && byte.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out g)
               && byte.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out b);

        // Se o texto tiver alpha, tenta parsear também
        if (parts.Length > 3)
            ok &= byte.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out a);

        result = ok ? new Rgba(r, g, b, a) : default;
        return ok;
    }
}

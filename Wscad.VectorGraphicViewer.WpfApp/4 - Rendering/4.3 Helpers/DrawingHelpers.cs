namespace Wscad.VectorGraphicViewer.WpfApp.Rendering.Helpers;

using System.Windows.Controls;
using System.Windows.Media;
using Wscad.VectorGraphicViewer.Domain.Entities;

internal static class DrawingHelpers
{
    // Calculates transform (world → screen) while preserving aspect ratio and flipping Y upwards
    public static (Func<double, double> Tx, Func<double, double> Ty) CreateTransform(
        Canvas surface, IEnumerable<(double x, double y)> worldPoints, double padding = 10)
    {
        double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
        double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;

        foreach (var (x, y) in worldPoints)
        {
            if (x < minX) minX = x; if (x > maxX) maxX = x;
            if (y < minY) minY = y; if (y > maxY) maxY = y;
        }

        minX -= padding; maxX += padding;
        minY -= padding; maxY += padding;

        double ww = surface.ActualWidth > 0 ? surface.ActualWidth : surface.RenderSize.Width;
        double wh = surface.ActualHeight > 0 ? surface.ActualHeight : surface.RenderSize.Height;
        if (ww <= 0 || wh <= 0) { ww = 400; wh = 240; }

        double worldW = Math.Max(maxX - minX, 1e-6);
        double worldH = Math.Max(maxY - minY, 1e-6);

        double scaleX = ww / worldW;
        double scaleY = wh / worldH;
        double scale = Math.Min(scaleX, scaleY);

        double offsetX = (ww - worldW * scale) / 2.0;
        double offsetY = (wh - worldH * scale) / 2.0;

        double Tx(double x) => (x - minX) * scale + offsetX;
        double Ty(double y) => (maxY - y) * scale + offsetY;

        return (Tx, Ty);
    }

    public static (Brush stroke, Brush fill) BuildBrushes(Primitive p)
    {
        var color = Color.FromArgb(
            p.Color?.A ?? 255,
            p.Color?.R ?? 0,
            p.Color?.G ?? 0,
            p.Color?.B ?? 0);

        var stroke = new SolidColorBrush(color);

        if (p.Filled ?? false)
        {
            const byte MinFillAlpha = 180;
            byte a = color.A < MinFillAlpha ? MinFillAlpha : color.A;
            return (stroke, new SolidColorBrush(Color.FromArgb(a, color.R, color.G, color.B)));
        }

        return (stroke, Brushes.Transparent);
    }
}
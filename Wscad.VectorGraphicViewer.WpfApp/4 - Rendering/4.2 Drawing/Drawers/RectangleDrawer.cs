#if false
// To enable Rectangle drawing, change `#if false` to `#if true` (or remove this guard).

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;

namespace Wscad.VectorGraphicViewer.WpfApp.Rendering
{
    public sealed class RectangleDrawer : IPrimitiveDrawer
    {
        public PrimitiveTypeEnum Kind => PrimitiveTypeEnum.Rectangle;

        public bool CanDraw(Primitive p)
            => p.Kind == PrimitiveTypeEnum.Rectangle && p.A is not null && p.B is not null;

        public void Draw(Canvas surface, Primitive p)
        {
            if (!CanDraw(p)) return;

            // Brush setup (consistent with other drawers)
            Color color = Color.FromArgb(
                (byte)(p.Color?.A ?? 255),
                (byte)(p.Color?.R ?? 0),
                (byte)(p.Color?.G ?? 0),
                (byte)(p.Color?.B ?? 0));

            SolidColorBrush stroke = new SolidColorBrush(color);
            Brush fill = (p.Filled ?? false)
                ? new SolidColorBrush(Color.FromArgb(Math.Max((byte)220, color.A), color.R, color.G, color.B))
                : Brushes.Transparent;

            // World-space bbox (from A and B)
            double ax = p.A.Value.X, ay = p.A.Value.Y;
            double bx = p.B.Value.X, by = p.B.Value.Y;

            double minX = Math.Min(ax, bx);
            double maxX = Math.Max(ax, bx);
            double minY = Math.Min(ay, by);
            double maxY = Math.Max(ay, by);

            // Padding
            const double padding = 10.0;
            minX -= padding; maxX += padding;
            minY -= padding; maxY += padding;

            // Canvas size
            double ww = surface.ActualWidth > 0 ? surface.ActualWidth : surface.RenderSize.Width;
            double wh = surface.ActualHeight > 0 ? surface.ActualHeight : surface.RenderSize.Height;
            if (ww <= 0 || wh <= 0) { ww = 400; wh = 240; }

            double worldW = Math.Max(maxX - minX, 1e-6);
            double worldH = Math.Max(maxY - minY, 1e-6);

            double scaleX = ww / worldW;
            double scaleY = wh / worldH;
            double scale = Math.Min(1.0, Math.Min(scaleX, scaleY)); // não ampliar além de 1.0 (mesma lógica usada)

            double offsetX = (ww - worldW * scale) / 2.0;
            double offsetY = (wh - worldH * scale) / 2.0;

            // Converte cantos reais (sem padding) para tela
            double rxMin = (Math.Min(ax, bx) - minX) * scale + offsetX;
            double rxMax = (Math.Max(ax, bx) - minX) * scale + offsetX;
            double ryMin = (maxY - Math.Max(ay, by)) * scale + offsetY; // invertendo Y
            double ryMax = (maxY - Math.Min(ay, by)) * scale + offsetY;

            double rectW = Math.Max(rxMax - rxMin, 0);
            double rectH = Math.Max(ryMax - ryMin, 0);

            Rectangle rect = new Rectangle
            {
                Width = rectW,
                Height = rectH,
                Stroke = stroke,
                StrokeThickness = 2,
                Fill = fill
            };

            Canvas.SetLeft(rect, rxMin);
            Canvas.SetTop(rect, ryMin);
            surface.Children.Add(rect);
        }
    }
}
#endif
namespace Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Drawers;

using System.Windows.Controls;
using System.Windows.Shapes;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Helpers;

public sealed class CircleDrawer : IPrimitiveDrawer
{
    public PrimitiveTypeEnum Kind => PrimitiveTypeEnum.Circle;

    public bool CanDraw(Primitive p) => p.Center is not null && p.Radius is double r && r > 0;

    public void Draw(Canvas surface, Primitive p)
    {
        var (stroke, fill) = DrawingHelpers.BuildBrushes(p);

        double r = p.Radius!.Value;
        double cx = p.Center!.Value.X;
        double cy = p.Center.Value.Y;

        // cantos do bounding box do círculo para a transformação
        var points = new (double x, double y)[]
        {
        (cx - r, cy - r), (cx + r, cy + r), (cx + r, cy - r), (cx - r, cy + r)
        };
        var (Tx, Ty) = DrawingHelpers.CreateTransform(surface, points);

        // escala local (px por 1 unidade do "mundo")
        double localScaleX = Math.Abs(Tx(cx + 1) - Tx(cx));
        if (localScaleX <= 0 || double.IsNaN(localScaleX) || double.IsInfinity(localScaleX))
            localScaleX = 1; // fallback seguro

        double diam = 2 * r * localScaleX;
        diam = Math.Max(diam, 10); // nunca menor que 10px

        double Cx = Tx(cx);
        double Cy = Ty(cy);

        // --- Garantir stroke visível quando não estiver preenchido ---
        // Se o fill for transparente, assegure um alpha mínimo para o traço
        if (fill == System.Windows.Media.Brushes.Transparent && stroke is System.Windows.Media.SolidColorBrush sb)
        {
            const byte MinStrokeA = 180;
            var c = sb.Color;
            if (c.A < MinStrokeA)
            {
                c = System.Windows.Media.Color.FromArgb(MinStrokeA, c.R, c.G, c.B);
                stroke = new System.Windows.Media.SolidColorBrush(c);
            }
        }

        var ellipse = new Ellipse
        {
            Width = diam,
            Height = diam,
            Stroke = stroke,
            StrokeThickness = 2,
            Fill = fill
        };

        Canvas.SetLeft(ellipse, Cx - diam / 2);
        Canvas.SetTop(ellipse, Cy - diam / 2);
        surface.Children.Add(ellipse);
    }
}
namespace Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Drawing.Drawers;

using System.Windows.Controls;
using System.Windows.Shapes;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Drawing.Contracts;
using Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Helpers;

public sealed class CircleDrawer : IPrimitiveDrawer
{
    public PrimitiveTypeEnum Kind => PrimitiveTypeEnum.Circle;

    public bool CanDraw(Primitive p) => p.Center is not null && p.Radius is double r && r > 0;

    public void Draw(Canvas surface, Primitive p)
    {
        surface.Children.Clear();
        var (stroke, fill) = DrawingHelpers.BuildBrushes(p);

        double r = p.Radius!.Value;
        var cx = p.Center!.Value.X;
        var cy = p.Center.Value.Y;

        var points = new (double x, double y)[]
        {
            (cx - r, cy - r), (cx + r, cy + r), (cx + r, cy - r), (cx - r, cy + r)
        };
        var (Tx, Ty) = DrawingHelpers.CreateTransform(surface, points);

        double diam = 2 * r * (Tx(cx + 1) - Tx(cx));
        diam = Math.Max(diam, 10);

        double Cx = Tx(cx);
        double Cy = Ty(cy);

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
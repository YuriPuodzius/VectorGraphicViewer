namespace Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Drawers;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Helpers;

public sealed class TriangleDrawer : IPrimitiveDrawer
{
    public PrimitiveTypeEnum Kind => PrimitiveTypeEnum.Triangle;

    public bool CanDraw(Primitive p) => p.A is not null && p.B is not null && p.C is not null;

    public void Draw(Canvas surface, Primitive p)
    {
        var (stroke, fill) = DrawingHelpers.BuildBrushes(p);

        var ptsWorld = new (double x, double y)[]
        {
            (p.A!.Value.X, p.A.Value.Y),
            (p.B!.Value.X, p.B.Value.Y),
            (p.C!.Value.X, p.C.Value.Y),
        };
        var (Tx, Ty) = DrawingHelpers.CreateTransform(surface, ptsWorld);

        var poly = new Polygon
        {
            Stroke = stroke,
            StrokeThickness = 2,
            Fill = fill,
            Points = new PointCollection
            {
                new System.Windows.Point(Tx(p.A.Value.X), Ty(p.A.Value.Y)),
                new System.Windows.Point(Tx(p.B.Value.X), Ty(p.B.Value.Y)),
                new System.Windows.Point(Tx(p.C.Value.X), Ty(p.C.Value.Y)),
            }
        };
        surface.Children.Add(poly);
    }
}
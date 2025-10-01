namespace Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Drawers;

using System.Windows.Controls;
using System.Windows.Shapes;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Helpers;

public sealed class LineDrawer : IPrimitiveDrawer
{
    public PrimitiveTypeEnum Kind => PrimitiveTypeEnum.Line;

    public bool CanDraw(Primitive p) => p.A is not null && p.B is not null;

    public void Draw(Canvas surface, Primitive p)
    {
        var (stroke, _) = DrawingHelpers.BuildBrushes(p);

        var points = new (double x, double y)[] { (p.A!.Value.X, p.A.Value.Y), (p.B!.Value.X, p.B.Value.Y) };
        var (Tx, Ty) = DrawingHelpers.CreateTransform(surface, points);

        var line = new Line
        {
            X1 = Tx(p.A.Value.X),
            Y1 = Ty(p.A.Value.Y),
            X2 = Tx(p.B.Value.X),
            Y2 = Ty(p.B.Value.Y),
            Stroke = stroke,
            StrokeThickness = 2
        };
        surface.Children.Add(line);
    }
}
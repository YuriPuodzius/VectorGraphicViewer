namespace Wscad.VectorGraphicViewer.WpfApp.Rendering.Orchestration;

using System.Windows.Controls;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;

public sealed class PrimitiveRenderCoordinator
{
    private readonly IReadOnlyDictionary<PrimitiveTypeEnum, IPrimitiveDrawer> _byKind;

    public PrimitiveRenderCoordinator(IEnumerable<IPrimitiveDrawer> drawers)
        => _byKind = drawers.ToDictionary(d => d.Kind);

    public void Render(Canvas surface, Primitive p)
    {
        if (surface == null || p == null)
            return;

        surface.Children.Clear();

        if (_byKind.TryGetValue(p.Kind, out var drawer) && drawer.CanDraw(p))
            drawer.Draw(surface, p);
    }
}
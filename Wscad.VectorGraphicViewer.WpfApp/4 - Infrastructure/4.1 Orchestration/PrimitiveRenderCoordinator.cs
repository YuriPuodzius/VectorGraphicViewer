using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Drawing.Contracts;

namespace Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Orchestration;

public sealed class PrimitiveRenderCoordinator
{
    private readonly IReadOnlyDictionary<PrimitiveTypeEnum, IPrimitiveDrawer> _byKind;

    public PrimitiveRenderCoordinator(IEnumerable<IPrimitiveDrawer> drawers)
        => _byKind = drawers.ToDictionary(d => d.Kind);

    public void Render(Canvas surface, Primitive p)
    {
        if (_byKind.TryGetValue(p.Kind, out var d) && d.CanDraw(p))
            d.Draw(surface, p);
        else
            surface.Children.Clear();
    }
}
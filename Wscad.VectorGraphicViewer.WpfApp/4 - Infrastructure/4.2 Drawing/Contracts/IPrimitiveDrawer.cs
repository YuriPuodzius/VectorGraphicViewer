namespace Wscad.VectorGraphicViewer.WpfApp.Infrastructure.Drawing.Contracts;

using System.Windows.Controls;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;

public interface IPrimitiveDrawer
{
    PrimitiveTypeEnum Kind { get; }

    // Se os dados mínimos existem (ex.: A e B pra linha)
    bool CanDraw(Primitive p);

    // Desenha no Canvas (inclui o fit mundo->tela)
    void Draw(Canvas surface, Primitive p);
}
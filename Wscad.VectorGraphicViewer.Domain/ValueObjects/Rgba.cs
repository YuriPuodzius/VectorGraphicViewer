namespace Wscad.VectorGraphicViewer.Domain.ValueObjects;

public struct Rgba
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

    public Rgba(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public override string ToString() => $"{R}; {G}; {B}; {A}";
}
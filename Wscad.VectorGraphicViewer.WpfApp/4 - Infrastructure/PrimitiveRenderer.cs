//using System;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;
//using Wscad.VectorGraphicViewer.Domain.Entities;
//using Wscad.VectorGraphicViewer.Domain.Enums;

//namespace Wscad.VectorGraphicViewer.WpfApp.Infrastructure
//{
//    public static class PrimitiveRenderer
//    {
//        public static void Render(Canvas surface, Primitive p)
//        {
//            surface.Children.Clear();

//            // ---------- Cor base ----------
//            var color = Color.FromArgb(
//                p.Color?.A ?? 255,
//                p.Color?.R ?? 0,
//                p.Color?.G ?? 0,
//                p.Color?.B ?? 0);

//            // stroke fiel ao JSON, mas garantindo visibilidade mínima
//            const byte MinStrokeAlpha = 180;
//            byte strokeA = color.A < MinStrokeAlpha ? MinStrokeAlpha : color.A;
//            var strokeColor = Color.FromArgb(strokeA, color.R, color.G, color.B);
//            var stroke = new SolidColorBrush(strokeColor);

//            // fill: só se Filled==true
//            Brush fill;
//            if (p.Filled ?? false)
//            {
//                const byte MinFillAlpha = 220;
//                byte a = color.A < MinFillAlpha ? MinFillAlpha : color.A;
//                var strongFill = Color.FromArgb(a, color.R, color.G, color.B);
//                fill = new SolidColorBrush(strongFill);
//            }
//            else
//                fill = Brushes.Transparent;

//            // ---------- 1) Bounding-box no "mundo" ----------
//            double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
//            double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;

//            void Consider(double x, double y)
//            {
//                if (x < minX) minX = x;
//                if (x > maxX) maxX = x;
//                if (y < minY) minY = y;
//                if (y > maxY) maxY = y;
//            }

//            switch (p.Kind)
//            {
//                case PrimitiveTypeEnum.Line when p.A is not null && p.B is not null:
//                    Consider(p.A.Value.X, p.A.Value.Y);
//                    Consider(p.B.Value.X, p.B.Value.Y);
//                    break;

//                case PrimitiveTypeEnum.Circle when p.Center is not null && p.Radius is double rc && rc > 0:
//                    Consider(p.Center.Value.X - rc, p.Center.Value.Y - rc);
//                    Consider(p.Center.Value.X + rc, p.Center.Value.Y + rc);
//                    break;

//                case PrimitiveTypeEnum.Triangle when p.A is not null && p.B is not null && p.C is not null:
//                    Consider(p.A.Value.X, p.A.Value.Y);
//                    Consider(p.B.Value.X, p.B.Value.Y);
//                    Consider(p.C.Value.X, p.C.Value.Y);
//                    break;

//                default:
//                    return; // nada a desenhar
//            }

//            // Margem
//            const double padding = 10.0;
//            minX -= padding; maxX += padding;
//            minY -= padding; maxY += padding;

//            // ---------- 2) Mundo -> Tela (mantém aspecto; Y para cima) ----------
//            double ww = surface.ActualWidth > 0 ? surface.ActualWidth : surface.RenderSize.Width;
//            double wh = surface.ActualHeight > 0 ? surface.ActualHeight : surface.RenderSize.Height;
//            if (ww <= 0 || wh <= 0) { ww = 400; wh = 240; }

//            double worldW = Math.Max(maxX - minX, 1e-6);
//            double worldH = Math.Max(maxY - minY, 1e-6);

//            double scaleX = ww / worldW;
//            double scaleY = wh / worldH;
//            double scale = Math.Min(1.0, Math.Min(scaleX, scaleY));

//            double offsetX = (ww - worldW * scale) / 2.0;
//            double offsetY = (wh - worldH * scale) / 2.0;

//            // ---------- 3) Desenho ----------
//            switch (p.Kind)
//            {
//                case PrimitiveTypeEnum.Line when p.A is not null && p.B is not null:
//                    {
//                        double x1 = (p.A.Value.X - minX) * scale + offsetX;
//                        double y1 = (maxY - p.A.Value.Y) * scale + offsetY;
//                        double x2 = (p.B.Value.X - minX) * scale + offsetX;
//                        double y2 = (maxY - p.B.Value.Y) * scale + offsetY;

//                        var line = new Line
//                        {
//                            X1 = x1,
//                            Y1 = y1,
//                            X2 = x2,
//                            Y2 = y2,
//                            Stroke = stroke,
//                            StrokeThickness = 2
//                        };
//                        surface.Children.Add(line);
//                        break;
//                    }

//                case PrimitiveTypeEnum.Circle when p.Center is not null && p.Radius is double r && r > 0:
//                    {
//                        double cx = (p.Center.Value.X - minX) * scale + offsetX;
//                        double cy = (maxY - p.Center.Value.Y) * scale + offsetY;

//                        double diam = 2 * r * scale;
//                        if (double.IsNaN(diam) || double.IsInfinity(diam) || diam <= 0) diam = 0;
//                        diam = Math.Max(diam, 10);

//                        var ellipse = new Ellipse
//                        {
//                            Width = diam,
//                            Height = diam,
//                            Stroke = stroke,
//                            StrokeThickness = 2,
//                            Fill = fill
//                        };

//                        Canvas.SetLeft(ellipse, cx - diam / 2.0);
//                        Canvas.SetTop(ellipse, cy - diam / 2.0);
//                        surface.Children.Add(ellipse);
//                        break;
//                    }

//                case PrimitiveTypeEnum.Triangle when p.A is not null && p.B is not null && p.C is not null:
//                    {
//                        var poly = new Polygon
//                        {
//                            Stroke = stroke,
//                            StrokeThickness = 2,
//                            Fill = fill,
//                            Points = new PointCollection
//                        {
//                            new System.Windows.Point((p.A.Value.X - minX) * scale + offsetX, (maxY - p.A.Value.Y) * scale + offsetY),
//                            new System.Windows.Point((p.B.Value.X - minX) * scale + offsetX, (maxY - p.B.Value.Y) * scale + offsetY),
//                            new System.Windows.Point((p.C.Value.X - minX) * scale + offsetX, (maxY - p.C.Value.Y) * scale + offsetY),
//                        }
//                        };
//                        surface.Children.Add(poly);
//                        break;
//                    }
//            }
//        }
//    }
//}
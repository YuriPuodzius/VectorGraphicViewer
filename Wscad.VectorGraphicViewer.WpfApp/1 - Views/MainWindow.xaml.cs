using System.Windows;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Contracts;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Drawing.Drawers;
using Wscad.VectorGraphicViewer.WpfApp.Rendering.Orchestration;
using Wscad.VectorGraphicViewer.WpfApp.ViewModels;

namespace Wscad.VectorGraphicViewer.WpfApp; 
    public partial class MainWindow : Window
    {
        private readonly PrimitiveRenderCoordinator _renderer;

        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            // Register drawers availables;
            _renderer = new PrimitiveRenderCoordinator(new IPrimitiveDrawer[]
            {
                new LineDrawer(),
                new CircleDrawer(),
                new TriangleDrawer()
                // new RectangleDrawer(),
                // new PolygonDrawer()
            });

            vm.DrawRequested += OnDrawRequested;
        }

        private void OnDrawRequested(Primitive p)
        {
            _renderer.Render(Surface, p);
        }
    }
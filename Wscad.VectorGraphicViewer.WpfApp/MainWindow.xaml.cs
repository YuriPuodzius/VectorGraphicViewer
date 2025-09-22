using System.Windows;
using Wscad.VectorGraphicViewer.WpfApp.Infrastructure;

namespace Wscad.VectorGraphicViewer.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.DrawRequested += p =>
            {
                PrimitiveRenderer.Render(Surface, p);
            };
        }
    }
}
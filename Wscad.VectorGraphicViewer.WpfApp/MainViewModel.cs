using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;
using Wscad.VectorGraphicViewer.Domain.Entities;
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.WpfApp.Infrastructure;

namespace Wscad.VectorGraphicViewer.WpfApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IPrimitiveService _svc;

        // Dropdown items
        public ObservableCollection<PrimitiveTypeEnum> AvailableKinds { get; } = new();

        private PrimitiveTypeEnum? _selectedKind;
        public PrimitiveTypeEnum? SelectedKind
        {
            get => _selectedKind;
            set
            {
                if (_selectedKind == value) return;
                _selectedKind = value;
                OnPropertyChanged();
                // habilita/desabilita o botão
                (DrawCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // Texto de debug mostrado na UI
        private string _debugText = "Select a primitive and click Draw.";
        public string DebugText
        {
            get => _debugText;
            set { _debugText = value; OnPropertyChanged(); }
        }

        // Comando do botão
        public ICommand DrawCommand { get; }

        public MainViewModel(IPrimitiveService svc)
        {
            _svc = svc;

            // carrega a lista uma vez
            foreach (var k in _svc.GetAvailablePrimitives())
                AvailableKinds.Add(k);

            DrawCommand = new RelayCommand(ExecuteDraw, CanExecuteDraw);
        }

        private bool CanExecuteDraw()
            => SelectedKind.HasValue && SelectedKind.Value != PrimitiveTypeEnum.Unknown;

        public event Action<Primitive>? DrawRequested;

        private void ExecuteDraw()
        {
            if (!SelectedKind.HasValue)
            {
                DebugText = "Please select a primitive.";
                return;
            }

            PrimitiveTypeEnum kind = SelectedKind.Value;
            Primitive? p = _svc.GetByType(kind);

            if (p is null)
            {
                DebugText = $"No data found for '{kind}'.";
                return;
            }

            // Monta um resumo amigável do objeto retornado
            DebugText = kind switch
            {
                PrimitiveTypeEnum.Line =>
                    $"Line: A=({p.A?.X:F2},{p.A?.Y:F2}) B=({p.B?.X:F2},{p.B?.Y:F2}) " +
                    $"Color=({p.Color?.R ?? 0},{p.Color?.G ?? 0},{p.Color?.B ?? 0},{p.Color?.A ?? 0})",

                PrimitiveTypeEnum.Circle =>
                    $"Circle: Center=({p.Center?.X:F2},{p.Center?.Y:F2}) Radius={p.Radius:F2} " +
                    $"Filled={(p.Filled ?? false ? "Yes" : "No")} " +
                    $"Color=({p.Color?.R ?? 0},{p.Color?.G ?? 0},{p.Color?.B ?? 0},{p.Color?.A ?? 0})",

                PrimitiveTypeEnum.Triangle =>
                    $"Triangle: A=({p.A?.X:F2},{p.A?.Y:F2}) B=({p.B?.X:F2},{p.B?.Y:F2}) C=({p.C?.X:F2},{p.C?.Y:F2}) " +
                    $"Filled={(p.Filled ?? false ? "Yes" : "No")} " +
                    $"Color=({p.Color?.R ?? 0},{p.Color?.G ?? 0},{p.Color?.B ?? 0},{p.Color?.A ?? 0})",

                _ => $"Primitive '{kind}' returned."
            };

            // avisa a view que tem que desenhar
            DrawRequested?.Invoke(p);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
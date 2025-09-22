using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;
using Wscad.VectorGraphicViewer.Domain.Enums;

namespace Wscad.VectorGraphicViewer.WpfApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IPrimitiveService _svc;

        public ObservableCollection<PrimitiveTypeEnum> AvailableKinds { get; } = new();

        private PrimitiveTypeEnum? _selectedKind;
        public PrimitiveTypeEnum? SelectedKind
        {
            get => _selectedKind;
            set { _selectedKind = value; OnPropertyChanged(); }
        }

        public MainViewModel(IPrimitiveService svc)
        {
            _svc = svc;

            // Load once (sync, simples)
            foreach (var k in _svc.GetAvailablePrimitives())
                AvailableKinds.Add(k);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
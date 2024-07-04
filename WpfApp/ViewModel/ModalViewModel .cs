using CoreModel.Model;
using System.ComponentModel;

namespace MVVM
{
    public class ModalViewModel : INotifyPropertyChanged
    {
        private SnifferData? _data;
        public SnifferData ModalData
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged(nameof(ModalData));
                }
            }
        }
        public ModalViewModel(SnifferData data)
        {
            ModalData = data;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using CoreModel.Model;
using System.ComponentModel;

namespace MVVM
{
    public class ModalViewModel : INotifyPropertyChanged
    {
        private StreamingData? _data;
        public StreamingData ModalData
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
        public ModalViewModel(StreamingData data)
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

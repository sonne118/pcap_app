using CoreModel.Model;
using GalaSoft.MvvmLight;

namespace wpfapp.ViewModel
{
    public class ModalViewModel : ViewModelBase
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
                    Set(ref _data, value);
                }
            }
        }
        public ModalViewModel(StreamingData data)
        {
            ModalData = data;
        }
    }
}

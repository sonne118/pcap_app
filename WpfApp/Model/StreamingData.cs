using GalaSoft.MvvmLight;

namespace CoreModel.Model
{
    public class StreamingData : ViewModelBase
    {
        private int _source_port;
        private int _dest_port;
        private string? _source_ip;
        private string? _dest_ip;
        private string? _source_mac;
        private string? _dest_mac;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
        public int Source_port
        {
            get => _source_port;
            set => Set(ref _source_port, value);
        }

        public int Dest_port
        {
            get => _dest_port;
            set => Set(ref _dest_port, value);
        }

        public string Source_ip
        {
            get => _source_ip;
            set => Set(ref _source_ip, value);
        }

        public string Dest_ip
        {
            get => _dest_ip;
            set => Set(ref _dest_ip, value);
        }

        public string Source_mac
        {
            get => _source_mac;
            set => Set(ref _source_mac, value);
        }

        public string Dest_mac
        {
            get => _dest_mac;
            set => Set(ref _dest_mac, value);
        }
    }
}
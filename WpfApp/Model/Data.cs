using GalaSoft.MvvmLight;

namespace CoreModel.Model
{
    public class Data : ViewModelBase
    {
        private int _id;
        private string? _source_ip;
        private string? _dest_ip;
        private string? _mac_source;
        private string? _mac_destin;
        private string? _user_agent;

        public int Id
        {
            get => _id;
            set => Set(ref _id, value);
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

        public string Mac_source
        {
            get => _mac_source;
            set => Set(ref _mac_source, value);
        }

        public string Mac_destin
        {
            get => _mac_destin;
            set => Set(ref _mac_destin, value);
        }

        public string User_agent
        {
            get => _user_agent;
            set => Set(ref _user_agent, value);
        }
    }
}
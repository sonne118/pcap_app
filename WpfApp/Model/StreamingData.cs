using GalaSoft.MvvmLight;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreModel.Model
{
    public class StreamingData : ViewModelBase
    {
        // privatr int id;
        private int source_port;
        private int dest_port;
        private string? source_ip = null;
        private string? dest_ip = null;
        private string? source_mac = null;
        private string? dest_mac = null;


        public int Source_port
        {
            get { return source_port; }
            set
            {
                source_port = value;
                OnPropertyChanged("source_port");
            }
        }
        public int Dest_port
        {
            get { return dest_port; }
            set
            {
                dest_port = value;
                OnPropertyChanged("dest_port");
            }
        }
        public string Source_ip
        {
            get { return source_ip; }
            set
            {
                source_ip = value;
                OnPropertyChanged("source_ip");
            }
        }

        public string Dest_ip
        {
            get { return dest_ip; }
            set
            {
                dest_ip = value;
                OnPropertyChanged("dest_ip");
            }
        }

        public string Source_mac
        {
            get { return source_mac; }
            set
            {
                source_mac = value;
                OnPropertyChanged("source_mac");
            }
        }

        public string Dest_mac
        {
            get { return dest_mac; }
            set
            {
                dest_mac = value;
                OnPropertyChanged("dest_mac");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
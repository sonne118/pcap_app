using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreModel.Model
{
    public class Data : INotifyPropertyChanged
    {
        public int id;
        public string source_ip;
        public string dest_ip;
        public string mac_source;
        public string mac_destin;


        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("id");
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
                OnPropertyChanged("Dest_ip");
            }
        }

        public string Mac_source
        {
            get { return mac_source; }
            set
            {
                mac_source = value;
                OnPropertyChanged("mac_source");
            }
        }

        public string Mac_destin
        {
            get { return mac_destin; }
            set
            {
                mac_destin = value;
                OnPropertyChanged("mac_destin");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
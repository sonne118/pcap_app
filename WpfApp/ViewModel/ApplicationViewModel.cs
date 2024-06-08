using CoreModel.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp.Model;
using WpfApp.Services.Helpers;


namespace MVVM
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Data selectedData;
        private readonly IEnumerable<PcapStruct> list;
        private readonly IAccumulateData _accumulateData;

        public ObservableCollection<Data> Data { get; set; }
        public Data SelectedData
        {
            get { return selectedData; }
            set
            {
                selectedData = value;
                OnPropertyChanged("SelectedData");
            }
        }

        public ApplicationViewModel(IAccumulateData accumulateData)
        {
            this._accumulateData = accumulateData;
            Data = new ObservableCollection<Data>();            
            Data.AddRange(_accumulateData.GetCurrerntData());

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

    public static class LinqExtensions
    {
        public static ICollection<Data> AddRange(this ICollection<Data> source, IEnumerable<PcapStruct> addSource)
        {
            foreach (var item in addSource)
            {
                source.Add(new Data
                {
                    Id = item.id,
                    Source_ip = item.source_ip,
                    Dest_ip = item.dest_ip,
                    Mac_source = item.mac_source,
                    Mac_destin = item.mac_destin,
                    User_agent = item.user_agent
                });                
            }
            return source;
        }
    }

}
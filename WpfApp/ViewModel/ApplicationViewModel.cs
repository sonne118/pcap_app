using CoreModel.Model;
using Services.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static dotnet_Ptr.dotnet_Ptr;

namespace MVVM
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Data selectedData;
        private readonly IEnumerable<myStruct> list;
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
        public static ICollection<Data> AddRange(this ICollection<Data> source, IEnumerable<myStruct> addSource)
        {
            foreach (myStruct item in addSource)
            {
                source.Add(new Data
                {
                    Id = item.id,
                    Source_ip = item.source_ip,
                    Dest_ip = item.dest_ip,
                    Mac_source = item.mac_source,
                    Mac_destin = item.mac_destin
                });                
            }
            return source;
        }
    }

}
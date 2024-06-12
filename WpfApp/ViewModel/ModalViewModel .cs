using CoreModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{

    public class ModalViewModel : INotifyPropertyChanged
    {
        private SnifferData _data;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

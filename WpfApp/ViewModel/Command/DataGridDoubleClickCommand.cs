using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpfapp.View;
using CoreModel.Model;

namespace wpfapp.ViewModel
{
    public class DataGridDoubleClickCommand
    {
        public RelayCommand<(RoutedEventArgs, object)> _showCommand { get; private set; }
      
        public DataGridDoubleClickCommand() 
        {
            _showCommand = new RelayCommand<(RoutedEventArgs, object)>(OnExecuted);
         
        }
        public ICommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                    _showCommand = new RelayCommand<(RoutedEventArgs, object)>(OnExecuted);
                return _showCommand;
            }
        }

        private void OnExecuted((RoutedEventArgs events, object commandParameter) args)
        {
            var e = args.events;
            var data = args.commandParameter as StreamingData;
            ModalViewModel viewModel = new ModalViewModel(data);
            wpfapp.View.ModalWindow modalWindow = new wpfapp.View.ModalWindow
            {
                DataContext = viewModel.ModalData
            };

            modalWindow.ShowDialog();
        }
    }
}


using CoreModel.Model;
using GalaSoft.MvvmLight.CommandWpf;
using wpfapp.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wpfapp.ViewModel
{
    public class DataGridDoubleClickCommand
    {
        public RelayCommand<(RoutedEventArgs, object)> _showCommand { get; private set; }
        private UserControl _mainWindow;
        public DataGridDoubleClickCommand(UserControl mainWindow)
        {
            _showCommand = new RelayCommand<(RoutedEventArgs, object)>(OnExecuted);
            _mainWindow = mainWindow;
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
            //var e = args.events;
            //var data = args.commandParameter as StreamingData;
            //ModalViewModel viewModel = new ModalViewModel(data);
            //ModalWindow modalWindow = new ModalWindow
            //{
            //    DataContext = viewModel.ModalData
            //};

            //modalWindow.ShowDialog();
        }
    }
}


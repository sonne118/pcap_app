using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


namespace wpfapp.ViewModel
{
    public class ClosingCommand  
    {
        private RelayCommand<object> _exitCommand;
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        public ICommand _relayCommandParams { get; set; }

        public ClosingCommand()
        {
            _exitCommand = new RelayCommand<object>(OnExit);
        }
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand<object>(OnExit);

                }
                return _exitCommand;
            }
        }

        void OnExit(object args)
        {
            if (args is CancelEventArgs e && _mainWindow is not null) //args is MainWindow
            {
                var result = MessageBox.Show(_mainWindow, "Do you want to close?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = result == MessageBoxResult.No;
                }
            }
        }
    }
}




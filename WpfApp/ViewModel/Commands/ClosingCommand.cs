using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MVVM
{
    public class ClosingCommand
    {
        private ICommand _exitCommand;
        private MainWindow _mainWindow;

        public ClosingCommand(MainWindow mainWindow)
        {
            _exitCommand = new RelayCommand<CancelEventArgs>(OnExit);
            _mainWindow = mainWindow;
        }
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    _exitCommand = new RelayCommand<CancelEventArgs>(OnExit);
                return _exitCommand;
            }
        }

        void OnExit(CancelEventArgs e)
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




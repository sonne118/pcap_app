using AutoMapper;
using CoreModel.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public ClosingCommand closingCommand;

        public DataGridDoubleClickCommand dataGridDoubleClickCommand;
        public ICommand OnClosingCommand { get { return closingCommand.ExitCommand; } }
        public ICommand OnDataGridDoubleClickCommand { get { return dataGridDoubleClickCommand.ShowCommand; } }
        
        public MainWindow(IBackgroundJobs<Snapshot> backgroundJobs,
                          IMapper mapper,
                          IDevices device,
                          IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            InitializeComponent();
            closingCommand = new ClosingCommand(this);
            dataGridDoubleClickCommand = new DataGridDoubleClickCommand(this);
            DataContext = new GridViewModel(backgroundJobs, device, mapper, scopeFactory);
        }       
    }
}


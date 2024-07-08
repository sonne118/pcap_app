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
        public ICommand OnClosingCommand { get; }

        public MainWindow(IBackgroundJobs<Snapshot> backgroundJobs,
                          IMapper mapper,
                          IDevices device,
                          IServiceScopeFactory scopeFactory)
        {           
            _scopeFactory = scopeFactory;
            InitializeComponent();
            OnClosingCommand = new ClosingCommand(this).ExitCommand;
            DataContext = new GridViewModel(backgroundJobs, device, mapper);                       
        }

        private void datagrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                CustomDataGrid? dg = sender as CustomDataGrid;
                SnifferData? _snifferData = dg?.SelectedItem as SnifferData;

                ModalViewModel viewModel = new ModalViewModel(_snifferData);
                ModalWindow modalWindow = new ModalWindow
                {
                    DataContext = viewModel.ModalData
                };

                modalWindow.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StopAsync(_stoppingCts.Token);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StartAsync(_stoppingCts.Token);
            }
        }

        private void cDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            var d = comboBox?.SelectedItem as string;
            var b = Int32.TryParse(d?.Substring(0, 1), out var dev);

            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                service.PutDevices(dev);
            }
        }
    }
}


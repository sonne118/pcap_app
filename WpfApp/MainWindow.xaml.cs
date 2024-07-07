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
using WpfApp.Services.Worker;


namespace MVVM
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        public MainWindow(IBackgroundJobs<Snapshot> backgroundJobs,
                          IMapper mapper,
                          IDevices device,
                          IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = new GridViewModel(backgroundJobs, device, mapper);
            _serviceProvider = serviceProvider;
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
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IHostedService>();
                service.StopAsync(_stoppingCts.Token);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
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

            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPutDevice>();
                service.PutDevices(dev);
            }
        }

        private void GridPcap_Closed(object sender, EventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service_h = scope.ServiceProvider.GetRequiredService<IHostedService>();
                var service_w = scope.ServiceProvider.GetRequiredService<Worker>();
                service_h.StopAsync(_stoppingCts.Token);
                service_w.StopAsync(_stoppingCts.Token);
            }
        }
    }

}
using AutoMapper;
using CoreModel.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;


namespace MVVM
{
    public partial class MainWindow : Window
    {

        private readonly IServiceProvider _serviceProvider;


        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public MainWindow(IBackgroundJobs<Snapshot> backgroundJobs, IMapper mapper, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = new GridViewModel(backgroundJobs, mapper);
            _serviceProvider = serviceProvider;

        }

        private void datagrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                CustomDataGrid dg = sender as CustomDataGrid;
                SnifferData _snifferData = dg.SelectedItem as SnifferData;

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

    }
}
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using wpfapp.Services.Worker;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow(IBackgroundJobs<Snapshot> backgroundJobs,
                          IMapper mapper,
                          IDevices device,
                          IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();
            DataContext = new CommandViewModel(backgroundJobs, device, mapper, scopeFactory, this);
        }

        private void ScrollDataGrid_ColumnDisplayIndexChanged(object sender, System.Windows.Controls.DataGridColumnEventArgs e)
        {

        }
    }
}


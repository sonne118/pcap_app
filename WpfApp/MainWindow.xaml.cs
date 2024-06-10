using AutoMapper;
using System.Windows;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        public MainWindow(IBackgroundJobs<Snapshot> _backgroundJobs, IMapper mapper)
        {
            InitializeComponent();
            DataContext = new GridViewModel(_backgroundJobs, mapper);

        }
    }
}
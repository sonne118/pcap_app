using AutoMapper;
using System.Windows;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        IMapper _mapper;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
      //  public MainWindow()
      //  { }
        public MainWindow(IBackgroundJobs<Snapshot> _backgroundJobs, IMapper mapper)
        {
            InitializeComponent();
            _mapper = mapper;
            // _backgroundJobs = backgroundJobs;
            DataContext = new ApplicationViewModel(_backgroundJobs,_mapper);
        
        }
    }
}
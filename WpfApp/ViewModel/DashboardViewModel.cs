using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MVVM;
using System.Diagnostics;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class DashboardViewModel//: GridViewModel
    {

        //private readonly IServiceScopeFactory _scopeFactory;
        //private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        //private readonly IMapper _mapper;
        private readonly IDevices _devices;
        public DashboardViewModel()
                               
        {
            Debug.WriteLine("DashboardViewModel"); 
        }
    }
}

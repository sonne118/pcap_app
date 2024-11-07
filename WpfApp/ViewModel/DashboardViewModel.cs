using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MVVM;
using System.Diagnostics;
using wpfapp.models;
using wpfapp.Services.BackgroundJob;
using wpfapp.Services.Worker;

namespace wpfapp.ViewModel
{
    public class DashboardViewModel
    {
        private readonly IDevices _devices;
        public DashboardViewModel()
                               
        {
            Debug.WriteLine("DashboardViewModel"); 
        }
    }
}

using AutoMapper;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;
using CoreModel.Model;

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
    }
}
using AutoMapper;
using System.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void datagrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                DataGrid dg = sender as DataGrid;
                DataRowView dr = dg.SelectedItem as DataRowView;

                ModalViewModel viewModel = new ModalViewModel("Data");
                ModalWindow modalWindow = new ModalWindow
                {
                    DataContext = viewModel
                };

                modalWindow.ShowDialog();
            }
        }
    }
}
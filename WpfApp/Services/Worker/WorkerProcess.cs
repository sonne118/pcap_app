﻿using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp.Services.Worker
{
    public class WorkerProcess : BackgroundService
    {
        [DllImport("sniffer_packages.dll")]
        extern static void fnCPPDLL();
        readonly private int timeout;
        readonly private string path;
        public WorkerProcess()
        {
            //path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "sniffer_packages.exe"));
            timeout = 10000;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            ThreadPool.QueueUserWorkItem(delegate
            {
                fnCPPDLL();
                //    var proc = new Process();
                //    try
                //    {
                //        proc.StartInfo.FileName = path;
                //        proc.StartInfo.UseShellExecute = false;
                //        proc.StartInfo.RedirectStandardInput = true;
                //        proc.Start();
                //    }
                //    finally
                //    {
                //        proc.WaitForExit();
                //        proc.Close();
                //    }
            });
            await Task.Delay(timeout, stoppingToken);
        }
    }

}
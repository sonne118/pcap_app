﻿using System.Diagnostics;

namespace worker
{
    public class WorkerProcess : BackgroundService
    {
        readonly private int timeout;
        readonly private string path;
        public WorkerProcess()
        {
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "sniffer_packages.exe"));
            timeout = 10000;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                var proc = new Process();
                try
                {
                    proc.StartInfo.FileName = path;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.Start();
                }
                finally
                {
                    proc.WaitForExit();
                    proc.Close();
                }
            });
            await Task.Delay(timeout, stoppingToken);
        }
    }

}
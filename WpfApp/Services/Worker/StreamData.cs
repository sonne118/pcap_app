﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfapp.IPC.Ptr;

namespace wpfapp.Services.Worker
{
    public class StreamData : IStreamData
    {
        public void GetStream(int d)
        {
            GetStreamPtr.StartStream(d);
        }
    }
}

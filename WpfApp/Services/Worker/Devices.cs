using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfapp.Services.IPC.Ptr;

namespace wpfapp.Services.Worker
{
    public class Devices : IDevices
    {
        public IEnumerable<string> GetDevices()
        {
            return DevicesPtr.GetAllDevices();
        }
    }
}

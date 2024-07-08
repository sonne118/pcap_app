using System.Collections.Generic;
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

using wpfapp.IPC.Ptr;

namespace wpfapp.Services.Worker
{
    public class PutDevice : IPutDevice
    {
        public void PutDevices(int dev)
        {
            PutdevPtr.PutDev(dev);
        }
    }
}

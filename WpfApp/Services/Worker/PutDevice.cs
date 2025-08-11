using wpfapp.IPC.Ptr;

namespace wpfapp.Services.Worker
{
    public class PutDevice : IPutDevice
    {
        public async Task? PutDeviceAsync(int dev)
        {
            PutdevPtr.PutDev(dev);
        }

        public void PutDevices(int dev)
        {
            throw new NotImplementedException();
        }
    }
}

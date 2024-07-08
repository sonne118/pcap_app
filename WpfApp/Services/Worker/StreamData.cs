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

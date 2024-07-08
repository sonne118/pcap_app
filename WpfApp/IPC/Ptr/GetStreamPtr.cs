using System.Runtime.InteropServices;
using System.Threading;

namespace wpfapp.IPC.Ptr
{
    public class GetStreamPtr
    {
        [DllImport("sniffer_packages.dll")]
        //[DllImport(@"D:\repo\test2\pcap_app12\2\pcap_app\x64\Debug\sniffer_packages.dll", EntryPoint =
        //"fnCPPDLL", CallingConvention = CallingConvention.StdCall)]
        extern static void fnCPPDLL(int dev);

        private static Thread _workerThread;
        public static void StartStream(int dev)
        {
            _workerThread = new Thread(() => fnCPPDLL(dev));
            _workerThread?.Start();
        }
    }
}

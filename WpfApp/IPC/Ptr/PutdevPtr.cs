using System.Runtime.InteropServices;

namespace wpfapp.IPC.Ptr
{
    public class PutdevPtr
    {
        //[DllImport("sniffer_packages.dll")]
        [DllImport(@"C:\repo\cppp\1\proj\pcap_app\x64\Debug\sniffer_packages.dll", EntryPoint =
        "fnPutdevCPPDLL", CallingConvention = CallingConvention.StdCall)]
        private extern static void fnPutdevCPPDLL(int dev);
        public static void PutDev(int dev)
        {
            fnPutdevCPPDLL(dev);
        }

    }
}

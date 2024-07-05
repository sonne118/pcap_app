using System.Runtime.InteropServices;

namespace wpfapp.IPC.Ptr
{
    public class PutdevPtr
    {
        [DllImport("sniffer_packages.dll")]
        //[DllImport(@"D:\repo\test2\pcap_app12\2\pcap_app\x64\Debug\sniffer_packages.dll", EntryPoint =
        //"fnPutdevCPPDLL", CallingConvention = CallingConvention.StdCall)]

        private extern static void fnPutdevCPPDLL(int dev);
        public static void PutDev(int dev)
        {
            fnPutdevCPPDLL(dev);
        }

    }
}

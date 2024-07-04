using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace wpfapp.Services.IPC.Ptr
{
    public static class DevicesPtr
    {
        //[DllImport("sniffer_packages.dll")]
        [DllImport(@"D:\repo\test2\pcap_app12\pcap_app\x64\Debug\sniffer_packages.dll", EntryPoint =
        "fnDevCPPDLL", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.SafeArray)]
        private extern static string[] fnDevCPPDLL();

        public static IEnumerable<string> GetAllDevices()
        {
            foreach (string item in fnDevCPPDLL())
                yield return item;
        }
    }
}

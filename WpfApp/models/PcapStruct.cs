using System.Runtime.InteropServices;

namespace WpfApp.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PcapStruct
    {
        public int id;
        public string source_ip;
        public string dest_ip;
        public string mac_source;
        public string mac_destin;
        public string user_agent;
    }
}

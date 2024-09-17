using System.Runtime.InteropServices;

namespace WpfApp.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Snapshot
    {
        public int id;
        public int source_port;

        public int dest_port;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string proto;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string source_ip;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string dest_ip;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string source_mac;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string dest_mac;
        public override string ToString() =>
            $"{source_ip}:{source_port} -> {dest_ip}:{dest_port}->source_mac:{source_mac}->dest_mac:{dest_mac}";
    }
}


using System.Runtime.InteropServices;

namespace dotnet_Ptr
{
    public class dotnet_Ptr
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

        [DllImport("pcap_reader.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Predict(out int size, [MarshalAs(UnmanagedType.LPStr)] string path2Image);

        public static List<PcapStruct> GetPredict(string path = "somepath")
        {
            var ptr = Predict(out int size, path);
            List<PcapStruct> results = new List<PcapStruct>();
            var structSize = Marshal.SizeOf(typeof(PcapStruct));
            for (var i = 0; i < size; i++)
            {
                var o = Marshal.PtrToStructure<PcapStruct>(ptr);
                results.Add(o);
                ptr += structSize;
            }
            return results;
        }
    }
}
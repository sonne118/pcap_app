using System.Collections.Generic;
using System.Runtime.InteropServices;
using WpfApp.Model;

namespace WpfApp.Services.Reader
{
    public class DotnetPtr
    {
        //[DllImport("pcap_reader.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern System.IntPtr Predict(out int size, [MarshalAs(UnmanagedType.LPStr)] string path2Image);

        //public static List<PcapStruct> GetPredict(string path = "somepath")
        //{
        //    var ptr = Predict(out int size, path);
        //    List<PcapStruct> results = new List<PcapStruct>();
        //    var structSize = Marshal.SizeOf(typeof(PcapStruct));
        //    for (var i = 0; i < size; i++)
        //    {
        //        var o = Marshal.PtrToStructure<PcapStruct>(ptr);
        //        results.Add(o);
        //        ptr += structSize;
        //    }
        //    return results;
        //}
    }
}
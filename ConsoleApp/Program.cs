using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace console
{
    internal class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct myStruct
        {          
            int id;
            string source_ip;
            string dest_ip;
            string mac_source;
            string mac_destin;
            string user_agent;
        }

        [DllImport("pcap_reader.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Predict(out int size, [MarshalAs(UnmanagedType.LPStr)] string path2Image);

        static void Main(string[] args)
        {
            var result1 = GetPredict();
            Console.WriteLine($"list of {result1.Count} structures");
            foreach (var o in result1)
                Console.WriteLine(o);
            Console.ReadLine();
        }

        public static List<myStruct> GetPredict(string path = "somepath")
        {
            var ptr = Predict(out int size, path);
            List<myStruct> results = new List<myStruct>();
            var structSize = Marshal.SizeOf(typeof(myStruct));
            for (var i = 0; i < size; i++)
            {
                var o = Marshal.PtrToStructure<myStruct>(ptr);
                results.Add(o);               
                ptr += structSize;
            }
            return results;
        }

    }

}


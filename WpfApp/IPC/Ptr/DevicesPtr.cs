using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace wpfapp.Services.IPC.Ptr
{
    public static class DevicesPtr
    {
        [DllImport("sniffer_packages.dll", EntryPoint = "fnDevCPPDLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public extern static void fnDevCPPDLL(IntPtr[]? data, int[]? sizes, ref int count);
        public static IEnumerable<string> GetAllDevices()
        {
            int count = 0;
            fnDevCPPDLL(null, null, ref count);
            IntPtr[] data = new IntPtr[count];
            int[] sizes = new int[count];

            fnDevCPPDLL(data, sizes, ref count);

            string[] res = new string[count];

            for (int i = 0; i < count; i++)
            {
                try
                {
                    res[i] = Marshal.PtrToStringAnsi(data[i], sizes[i]);
                    yield return res[i];
                }
                finally
                {
                    if (data[i] != IntPtr.Zero)
                    {
                        data[i] = IntPtr.Zero;
                        Marshal.FreeHGlobal(data[i]);
                    }
                }
            }
        }
    }
}

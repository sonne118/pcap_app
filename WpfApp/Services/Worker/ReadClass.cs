using System;
using System.IO;
using System.Runtime.InteropServices;
using WpfApp.Model;

namespace WpfApp.Services.Worker
{
    public static class ReadClass
    {
        public static unsafe Snapshot ReadMessage(BinaryReader stream)
        {
            Snapshot snapshot;
            byte[] buffer = new byte[Marshal.SizeOf(typeof(Snapshot))];

            stream.Read(buffer, 0, buffer.Length);
            fixed (byte* b = &buffer[0])
            {
                var p = new IntPtr(b);
                snapshot = Marshal.PtrToStructure<Snapshot>(p);
            }
            return snapshot;
        }
    }
}






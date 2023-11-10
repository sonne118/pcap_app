using System;
using System.Runtime.InteropServices;


namespace IntPtrSCharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var name = new Name();

            IntPtr _ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(ArrayStruct)) * 100);
            GetName(ref _ptr);

            IntPtr current = _ptr;
            Name[] rArray = new Name[3];

            for (int i = 0; i < 3; i++)
            {
                rArray[i] = new Name();
                Marshal.PtrToStructure(current, rArray[i]);
                //Marshal.FreeCoTaskMem( (IntPtr)Marshal.ReadInt32( current ));
                // Marshal.DestroyStructure(current, typeof(Name));
                int structsize = Marshal.SizeOf(rArray[i]);
                current = (IntPtr)((long)current + structsize);
            }
            Marshal.FreeCoTaskMem(_ptr);

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Element {0}: {1} {2}", i,
                   rArray[i].FirstName, rArray[i].LastName);
            }
            Console.ReadLine();
            Marshal.FreeHGlobal(_ptr);

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class Name
        {
            public String FirstName;
            public String LastName;

            public Name()
            {
                FirstName = "";
                LastName = "";
            }
        };

        [DllImport("marhsalCpp.dll")]
        public static extern void GetName(ref IntPtr outArray);


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ArrayStruct
        {
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string myarray;
            public int length;
        }

        [DllImport("marhsalCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetArray(ref IntPtr outArray);

        public static T ByteArrayToStructure<T>(byte[] bytearray)
        {
            int len = Marshal.SizeOf(typeof(T));
            IntPtr ptr = IntPtr.Zero;
            T obj;
            try
            {
                ptr = Marshal.AllocHGlobal(len);
                Marshal.Copy(bytearray, 0, ptr, len);
                obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return obj;
        }

        public static byte[] StructureToByteArray<T>(T obj)
        {
            int len = Marshal.SizeOf(typeof(T));
            var arr = new byte[len];
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(len);
                Marshal.StructureToPtr(obj, ptr, true);
                Marshal.Copy(ptr, arr, 0, len);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return arr;
        }


    }
}

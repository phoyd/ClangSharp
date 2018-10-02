using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDL
{
    internal unsafe class StringMarshaler : ICustomMarshaler
    {
        public const string LeaveAllocated = "LeaveAllocated";

        private static ICustomMarshaler
            _leaveAllocatedInstance = new StringMarshaler(true),
            _defaultInstance = new StringMarshaler(false);

        public static ICustomMarshaler GetInstance(string cookie)
        {
            switch (cookie)
            {
                case "LeaveAllocated":
                    return _leaveAllocatedInstance;

                default:
                    return _defaultInstance;
            }
        }

        private bool _leaveAllocated;

        public StringMarshaler(bool leaveAllocated)
        {
            _leaveAllocated = leaveAllocated;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var ptr = (byte*)pNativeData;
            while (*ptr != 0)
            {
                ptr++;
            }
            var bytes = new byte[ptr - (byte*)pNativeData];
            Marshal.Copy(pNativeData, bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj == null)
                return IntPtr.Zero;
            var str = ManagedObj as string;
            if (str == null)
            {
                throw new ArgumentException("ManagedObj must be a string.", "ManagedObj");
            }
            var bytes = Encoding.UTF8.GetBytes(str);
            var mem = Methods.SDL_malloc((ulong)(bytes.Length + 1));
            Marshal.Copy(bytes, 0, mem, bytes.Length);
            ((byte*)mem)[bytes.Length] = 0;
            return mem;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (!_leaveAllocated)
            {
                Methods.SDL_free(pNativeData);
            }
        }

        public int GetNativeDataSize()
        {
            return -1;
        }
    }
}

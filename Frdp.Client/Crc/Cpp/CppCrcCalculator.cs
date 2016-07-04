using System;
using System.Runtime.InteropServices;

namespace Frdp.Client.Crc.Cpp
{
    public class CppCrcCalculator : ICrcCalculator
    {
        public UInt16 Calculate(
            byte[] data,
            uint startIndex,
            uint length
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            unsafe
            {
                fixed (byte* p = data)
                {
                    return
                        DoCrcCalculate(
                            new IntPtr(p + startIndex),
                            length
                            );
                }
            }
        }

        [DllImport("PerformanceLibrary.dll")]
        public static extern ushort DoCrcCalculate(
            IntPtr input,
            uint length
        );

    }
}

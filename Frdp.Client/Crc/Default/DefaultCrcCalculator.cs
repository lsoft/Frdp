using System;

namespace Frdp.Client.Crc.Default
{
    class DefaultCrcCalculator : ICrcCalculator
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

            UInt16 crc = 0xffff;
            byte i;

            for (var cc = startIndex; cc < startIndex + length; cc++)
            {
                crc ^= (UInt16)(((UInt16)data[cc]) << 8);

                for (i = 0; i < 8; i++)
                    crc = (UInt16)(((crc & 0x8000) > 0) ? (crc << 1) ^ 0x1021 : crc << 1);
            }

            return crc;

        }

    }
}

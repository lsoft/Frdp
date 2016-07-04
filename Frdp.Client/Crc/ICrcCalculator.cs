using System;

namespace Frdp.Client.Crc
{
    public interface ICrcCalculator
    {
        UInt16 Calculate(
            byte[] data,
            uint startIndex,
            uint length
            );
    }
}
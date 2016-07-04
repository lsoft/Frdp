using System;
using System.IO;

namespace Frdp.Common.Block.Diff
{
    public class BlockDiff
    {
        private readonly byte[] _data;
        private readonly uint _startIndex;
        private readonly uint _length;

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public BlockDiff(
            int x,
            int y,
            byte[] data,
            uint startIndex,
            uint length
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            X = x;
            Y = y;

            _data = data;
            _startIndex = startIndex;
            _length = length;
        }

        public void OpenData(
            out byte[] data,
            out uint startIndex,
            out uint length
            )
        {
            data = _data;
            startIndex = _startIndex;
            length = _length;
        }

        public byte[] ExtractData(
            )
        {
            var result = new byte[_length];

            Array.ConstrainedCopy(_data, (int)_startIndex, result, 0, (int)_length);

            return
                result;
        }
    }
}
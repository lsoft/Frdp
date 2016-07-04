using System;
using System.Collections.Generic;
using System.IO;

namespace Frdp.Common.Block.Diff
{
    public class DiffContainer
    {
        private readonly List<BlockDiff> _diffs = new List<BlockDiff>();

        public uint BlockWidth
        {
            get;
            private set;
        }

        public uint BlockHeight
        {
            get;
            private set;
        }

        public int ImageWidth
        {
            get;
            private set;
        }

        public int ImageHeight
        {
            get;
            private set;
        }

        public int BlockCountHorizontal
        {
            get;
            private set;
        }

        public int BlockCountVertical
        {
            get;
            private set;
        }

        public int TotalBlockCount
        {
            get;
            private set;
        }

        public List<BlockDiff> Diffs
        {
            get
            {
                return
                    _diffs;
            }
        }

        public DiffContainer(
            uint blockWidth,
            uint blockHeight,
            int imageWidth,
            int imageHeight,
            int blockCountHorizontal,
            int blockCountVertical,
            int totalBlockCount
            )
        {
            BlockWidth = blockWidth;
            BlockHeight = blockHeight;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            BlockCountHorizontal = blockCountHorizontal;
            BlockCountVertical = blockCountVertical;
            TotalBlockCount = totalBlockCount;
        }

        public void AddDiff(
            BlockDiff diff
            )
        {
            if (diff == null)
            {
                throw new ArgumentNullException("diff");
            }

            _diffs.Add(diff);
        }

        public bool ContainsChanges()
        {
            return
                _diffs.Count > 0;
        }
    }
}
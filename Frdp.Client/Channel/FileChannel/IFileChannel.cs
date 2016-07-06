using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frdp.Client.Channel.FileChannel
{
    public interface IFileChannel : IDisposable
    {
        byte[] GetData(
            string filepath,
            long offset,
            int length
            );
    }
}

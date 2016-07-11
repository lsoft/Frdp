using System;
using System.IO;
using System.ServiceModel;
using Frdp.Common;
using Frdp.Wcf.Contract.FileChannel;

namespace Frdp.Server.Wcf.FileChannel
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class WcfFileService : IWcfFileChannel
    {
        private readonly ILogger _logger;

        public WcfFileService(
            ILogger logger
            )
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
        }

        public byte[] GetData(string filepath, long offset, int length)
        {
            byte[] result = null;

            try
            {
                if (File.Exists(filepath))
                {
                    using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        fs.Position = offset;

                        result = new byte[length];

                        fs.Read(result, 0, length);
                    }
                }
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }

            return
                result;
        }

        public void AppendData(
            string filepath,
            bool forceToCreateFolder,
            byte[] data
            )
        {
            if (forceToCreateFolder)
            {
                var fi = new FileInfo(filepath);
                var d = fi.Directory.FullName;

                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
            }

            try
            {
                FileStream fs;
                if (!File.Exists(filepath))
                {
                    fs = File.Create(filepath);
                }
                else
                {
                    fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                }

                using (fs)
                {
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }
        }
    }
}
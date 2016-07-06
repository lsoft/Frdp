using System;
using System.Runtime.Serialization;

namespace Frdp.Wcf.Contract.MainChannel
{
    [DataContract]
    public class Diff
    {
        [DataMember]
        public int X
        {
            get;
            private set;
        }

        [DataMember]
        public int Y
        {
            get;
            private set;
        }

        [DataMember]
        public byte[] Data
        {
            get;
            private set;
        }

        public Diff(int x, int y, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            X = x;
            Y = y;
            Data = data;
        }
    }
}
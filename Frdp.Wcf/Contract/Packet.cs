using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Frdp.Wcf.Contract
{
    [DataContract]
    public class Packet
    {
        [DataMember]
        public WcfClientSettings ClientSettings
        {
            get;
            private set;
        }

        [DataMember]
        public List<Diff> Diffs
        {
            get;
            private set;
        }

        public Packet(WcfClientSettings clientSettings, List<Diff> diffs)
        {
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }
            if (diffs == null)
            {
                throw new ArgumentNullException("diffs");
            }
            ClientSettings = clientSettings;
            Diffs = diffs;
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace Frdp.Wcf.Contract.MainChannel
{
    [DataContract]
    public class ServerCommands
    {
        [DataMember]
        public string XmlCommands
        {
            get;
            private set;
        }

        public ServerCommands()
        {
        }

        public ServerCommands(string xmlCommands)
        {
            if (xmlCommands == null)
            {
                throw new ArgumentNullException("xmlCommands");
            }

            XmlCommands = xmlCommands;
        }
    }
}

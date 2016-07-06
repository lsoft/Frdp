using System;
using System.Xml;
using Frdp.Common.Command;
using Frdp.Wcf.Contract.MainChannel;

namespace Frdp.Server.Wcf.MainChannel.Result
{
    public class WcfResultFactory : IWcfResultFactory
    {
        private readonly ICommandContainer _commandContainer;

        public WcfResultFactory(
            ICommandContainer commandContainer
            )
        {
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }
            _commandContainer = commandContainer;
        }

        public ServerCommands CreateServerCommands(
            )
        {
            var document = new XmlDocument();

            var declaration = document.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
            document.AppendChild(declaration);

            var cmdsNode = document.CreateElement("commands");
            document.AppendChild(cmdsNode);

            var nodes = _commandContainer.CreateXmlNodes(
                document
                );

            nodes.ForEach(j => cmdsNode.AppendChild(j));

            var result = new ServerCommands(
                document.OuterXml
                );

            return
                result;
        }
    }
}

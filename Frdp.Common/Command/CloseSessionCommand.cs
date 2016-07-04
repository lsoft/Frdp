using System;
using System.Xml;

namespace Frdp.Common.Command
{
    public class CloseSessionCommand : BaseCommand
    {

        public static string CommandType
        {
            get
            {
                return
                    "CloseSession";
            }
        }

        public CloseSessionCommand(
            )
        {
        }

        public CloseSessionCommand(
            XmlNode node
            )
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
        }

        public override XmlNode CreateXmlNode(
            XmlDocument document
            )
        {
            var node = document.CreateElement("command");

            var attribute = document.CreateAttribute(BaseCommand.AttributeName);
            attribute.InnerText = CloseSessionCommand.CommandType;
            node.Attributes.Append(attribute);

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                string.Empty;
        }
    }
}
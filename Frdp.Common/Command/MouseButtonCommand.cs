using System;
using System.IO;
using System.Xml;

namespace Frdp.Common.Command
{
    public class MouseButtonCommand : BaseCommand
    {
        public MouseButtonCommandEnum Action
        {
            get;
            private set;
        }

        public enum MouseButtonCommandEnum
        {
            LeftButtonDown,
            LeftButtonUp,
            RightButtonDown,
            RightButtonUp
        }

        public static string CommandType
        {
            get
            {
                return
                    "MouseButton";
            }
        }

        public MouseButtonCommand(
            MouseButtonCommandEnum action
            )
        {
            Action = action;
        }

        public MouseButtonCommand(
            XmlNode node
            )
        {
            var sa = node.SelectSingleNode("action").InnerText;

            MouseButtonCommandEnum action;
            if (!Enum.TryParse(
                sa,
                out action)
                )
            {
                throw new InvalidDataException(sa);
            }

            this.Action = action;
        }

        public override XmlNode CreateXmlNode(
            XmlDocument document
            )
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var node = document.CreateElement("command");

            var attribute = document.CreateAttribute(BaseCommand.AttributeName);
            attribute.InnerText = MouseButtonCommand.CommandType;
            node.Attributes.Append(attribute);

            node.InnerXml = string.Format(
                XmlBody,
                Action
                );

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                Action.ToString();
        }

        private const string XmlBody = @"
<action>{0}</action>
";

    }
}
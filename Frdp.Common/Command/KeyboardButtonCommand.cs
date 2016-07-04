using System;
using System.IO;
using System.Windows.Input;
using System.Xml;

namespace Frdp.Common.Command
{
    public class KeyboardButtonCommand : BaseCommand
    {
        public bool IsKeyDown
        {
            get;
            private set;
        }

        public Key Key
        {
            get;
            private set;
        }


        public static string CommandType
        {
            get
            {
                return
                    "KeyboardButton";
            }
        }

        public KeyboardButtonCommand(
            bool isKeyDown,
            Key key
            )
        {
            IsKeyDown = isKeyDown;
            Key = key;
        }

        public KeyboardButtonCommand(
            XmlNode node
            )
        {
            var sa = node.SelectSingleNode("key").InnerText;

            Key key;
            if (!Enum.TryParse(
                sa,
                out key)
                )
            {
                throw new InvalidDataException(sa);
            }

            this.IsKeyDown = bool.Parse(node.SelectSingleNode("iskeydown").InnerText);
            this.Key = key;
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
            attribute.InnerText = KeyboardButtonCommand.CommandType;
            node.Attributes.Append(attribute);

            node.InnerXml = string.Format(
                XmlBody,
                IsKeyDown,
                Key
                );

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                string.Format(
                    "{0} {1}", 
                    IsKeyDown ? "down" : "up",
                    Key
                    );
        }

        private const string XmlBody = @"
<iskeydown>{0}</iskeydown>
<key>{1}</key>
";

    }
}

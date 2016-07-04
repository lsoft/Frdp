using System;
using System.Drawing;
using System.Net.Configuration;
using System.Windows.Forms;
using System.Xml;
using OpenCvSharp.ML;

namespace Frdp.Common.Command
{
    public class MousePositionCommand : BaseCommand
    {
        public static string CommandType
        {
            get
            {
                return
                    "MousePosition";
            }
        }

        public double RelativeX
        {
            get;
            private set;
        }

        public double RelativeY
        {
            get;
            private set;
        }

        public MousePositionCommand(
            XmlNode node
            )
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            RelativeX = double.Parse(node.SelectSingleNode("x").InnerText);
            RelativeY = double.Parse(node.SelectSingleNode("y").InnerText);
        }

        public MousePositionCommand(
            double relativeX,
            double relativeY
            )
        {
            RelativeX = relativeX;
            RelativeY = relativeY;
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
            attribute.InnerText = MousePositionCommand.CommandType;
            node.Attributes.Append(attribute);

            node.InnerXml = string.Format(
                XmlBody,
                RelativeX,
                RelativeY
                );

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                string.Format(
                    "x:{0} y:{1}",
                    RelativeX,
                    RelativeY
                    );
        }

        private const string XmlBody = @"
<x>{0}</x>
<y>{1}</y>
";
    }
}
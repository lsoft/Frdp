using System;
using System.Xml;
using Frdp.Common.Settings;

namespace Frdp.Common.Command
{
    public class ChangeClientSettingsCommand : BaseCommand
    {
        private readonly IClientSettingsContainer _clientSettings;

        public uint BlockWidth
        {
            get
            {
                return
                    _clientSettings.BlockWidth;
            }
        }

        public uint BlockHeight
        {
            get
            {
                return
                    _clientSettings.BlockHeight;
            }
        }

        public int ScaleFactorX
        {
            get
            {
                return
                    _clientSettings.ScaleFactorX;
            }
        }

        public int ScaleFactorY
        {
            get
            {
                return
                    _clientSettings.ScaleFactorY;
            }
        }

        public TimeSpan TimeoutBetweenFrames
        {
            get
            {
                return
                    _clientSettings.TimeoutBetweenFrames;
            }
        }

        public byte Mask
        {
            get
            {
                return
                    _clientSettings.Mask;
            }
        }

        public static string CommandType
        {
            get
            {
                return
                    "ChangeClientSettings";
            }
        }

        public ChangeClientSettingsCommand(
            IClientSettingsContainer clientSettings
            )
        {
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }
            _clientSettings = clientSettings;
        }

        public ChangeClientSettingsCommand(
            XmlNode node
            )
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            var blockWidth = uint.Parse(node.SelectSingleNode("BlockWidth").InnerText);
            var blockHeight = uint.Parse(node.SelectSingleNode("BlockHeight").InnerText);
            var scaleFactorX = int.Parse(node.SelectSingleNode("ScaleFactorX").InnerText);
            var scaleFactorY = int.Parse(node.SelectSingleNode("ScaleFactorY").InnerText);
            var timeoutBetweenFrames = TimeSpan.FromMilliseconds(int.Parse(node.SelectSingleNode("TimeoutBetweenFrames").InnerText));
            var mask = byte.Parse(node.SelectSingleNode("Mask").InnerText);

            var clientSettings = new ClientSettings(
                blockWidth,
                blockHeight,
                scaleFactorX,
                scaleFactorY,
                timeoutBetweenFrames,
                mask
                );

            _clientSettings = clientSettings;
        }

        public override XmlNode CreateXmlNode(
            XmlDocument document
            )
        {
            var node = document.CreateElement("command");

            var attribute = document.CreateAttribute(BaseCommand.AttributeName);
            attribute.InnerText = ChangeClientSettingsCommand.CommandType;
            node.Attributes.Append(attribute);

            node.InnerXml = string.Format(
                XmlBody,
                BlockWidth,
                BlockHeight,
                ScaleFactorX,
                ScaleFactorY,
                TimeoutBetweenFrames.TotalMilliseconds,
                Mask
                );

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                string.Format(
                    "BlockWidth {0}, BlockHeight {1}, ScaleFactorX {2}, ScaleFactorY {3}, TimeoutBetweenFrames {4}, Mask {5}",
                    BlockWidth,
                    BlockHeight,
                    ScaleFactorX,
                    ScaleFactorY,
                    TimeoutBetweenFrames,
                    Mask
                    );
        }

        private const string XmlBody = @"
<BlockWidth>{0}</BlockWidth>
<BlockHeight>{1}</BlockHeight>
<ScaleFactorX>{2}</ScaleFactorX>
<ScaleFactorY>{3}</ScaleFactorY>
<TimeoutBetweenFrames>{4}</TimeoutBetweenFrames>
<Mask>{5}</Mask>
";
    }
}
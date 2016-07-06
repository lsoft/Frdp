using System;
using System.Xml;

namespace Frdp.Common.Command
{
    public class StartFileTaskCommand : BaseCommand
    {
        public string ServerFilePath
        {
            get;
            private set;
        }

        public string ClientFilePath
        {
            get;
            private set;
        }

        public long TotalFileSize
        {
            get;
            private set;
        }

        public bool ForceToCreateFolder
        {
            get;
            private set;
        }

        public bool ForceToDeleteFile
        {
            get;
            private set;
        }

        public static string CommandType
        {
            get
            {
                return
                    "StartFileTask";
            }
        }

        public StartFileTaskCommand(
            string serverFilePath, 
            string clientFilePath, 
            long totalFileSize,
            bool forceToCreateFolder,
            bool forceToDeleteFile
            )
        {
            ServerFilePath = serverFilePath;
            ClientFilePath = clientFilePath;
            TotalFileSize = totalFileSize;
            ForceToCreateFolder = forceToCreateFolder;
            ForceToDeleteFile = forceToDeleteFile;
        }

        public StartFileTaskCommand(
            XmlNode node
            )
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            ServerFilePath = node.SelectSingleNode("serverFilePath").InnerText;
            ClientFilePath = node.SelectSingleNode("clientFilePath").InnerText;
            TotalFileSize = long.Parse(node.SelectSingleNode("totalFileSize").InnerText);
            ForceToCreateFolder = bool.Parse(node.SelectSingleNode("forceToCreateFolder").InnerText);
            ForceToDeleteFile = bool.Parse(node.SelectSingleNode("forceToDeleteFile").InnerText);
        }

        public override XmlNode CreateXmlNode(
            XmlDocument document
            )
        {
            var node = document.CreateElement("command");

            var attribute = document.CreateAttribute(BaseCommand.AttributeName);
            attribute.InnerText = StartFileTaskCommand.CommandType;
            node.Attributes.Append(attribute);

            node.InnerXml = string.Format(
                XmlBody,
                ServerFilePath,
                ClientFilePath,
                TotalFileSize,
                ForceToCreateFolder,
                ForceToDeleteFile
                );

            return
                node;
        }

        public override string ArgsToString()
        {
            return
                string.Format(
                    "serverFilePath {0}, clientFilePath {1}, TotalFileSize {2}, ForceToCreateFolder {3}, ForceToDeleteFile {4}",
                    ServerFilePath,
                    ClientFilePath,
                    TotalFileSize,
                    ForceToCreateFolder,
                    ForceToDeleteFile
                    );
        }

        private const string XmlBody = @"
<serverFilePath>{0}</serverFilePath>
<clientFilePath>{1}</clientFilePath>
<totalFileSize>{2}</totalFileSize>
<forceToCreateFolder>{3}</forceToCreateFolder>
<forceToDeleteFile>{4}</forceToDeleteFile>
";

    }
}
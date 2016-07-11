using System;
using System.Xml;

namespace Frdp.Common.Command
{
    public class StartFileTaskCommand : BaseCommand
    {
        public bool DownloadFile
        {
            get;
            private set;
        }

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
            bool downloadFile,
            string serverFilePath, 
            string clientFilePath, 
            long totalFileSize,
            bool forceToCreateFolder,
            bool forceToDeleteFile
            )
        {
            DownloadFile = downloadFile;
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

            DownloadFile = bool.Parse(node.SelectSingleNode("downloadfile").InnerText);
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
                DownloadFile,
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
                    "DownloadFile {0}, serverFilePath {1}, clientFilePath {2}, TotalFileSize {3}, ForceToCreateFolder {4}, ForceToDeleteFile {5}",
                    DownloadFile,
                    ServerFilePath,
                    ClientFilePath,
                    TotalFileSize,
                    ForceToCreateFolder,
                    ForceToDeleteFile
                    );
        }

        private const string XmlBody = @"
<downloadfile>{0}</downloadfile>
<serverFilePath>{1}</serverFilePath>
<clientFilePath>{2}</clientFilePath>
<totalFileSize>{3}</totalFileSize>
<forceToCreateFolder>{4}</forceToCreateFolder>
<forceToDeleteFile>{5}</forceToDeleteFile>
";

    }
}
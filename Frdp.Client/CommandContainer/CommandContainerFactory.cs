using System;
using System.Xml;
using Frdp.Common.Command;
using Frdp.Wcf;
using Frdp.Wcf.Contract;
using Frdp.Wcf.Contract.MainChannel;

namespace Frdp.Client.CommandContainer
{
    public class CommandContainerFactory : ICommandContainerFactory
    {
        private readonly ICommandFactory _commandFactory;

        public CommandContainerFactory(
            ICommandFactory commandFactory
            )
        {
            if (commandFactory == null)
            {
                throw new ArgumentNullException("commandFactory");
            }
            _commandFactory = commandFactory;
        }

        public ICommandContainer CreateCommandContainer(
            ServerCommands serverCommands
            )
        {
            if (serverCommands == null)
            {
                throw new ArgumentNullException("serverCommands");
            }

            var result = new Common.Command.Container.CommandContainer();

            var d = new XmlDocument();
            d.LoadXml(serverCommands.XmlCommands);

            foreach (XmlNode xmlCmd in d.SelectNodes("/commands/command"))
            {
                var xmlCmdTypeName = xmlCmd.Attributes[BaseCommand.AttributeName].Value;

                var cmd = _commandFactory.CreateCommand(
                    xmlCmdTypeName,
                    xmlCmd
                    );

                result.AddCommand(cmd);
            }

            return
                result;
        }
    }
}
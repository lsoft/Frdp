using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Frdp.Common.Command.Container
{

    public class CommandContainer : ICommandContainer
    {
        private readonly List<BaseCommand> _list = new List<BaseCommand>();

        private readonly object _locker = new object();

        public CommandContainer(
            )
        {
        }

        public void AddCommand(
            BaseCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            lock (_locker)
            {
                _list.Add(command);
            }
        }

        public List<XmlNode> CreateXmlNodes(
            XmlDocument document
            )
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            List<BaseCommand> clone;
            lock (_locker)
            {
                clone = new List<BaseCommand>(_list);
                _list.Clear();
            }

            var result = new List<XmlNode>();

            foreach (var cmd in clone)
            {
                var xmlNode = cmd.CreateXmlNode(
                    document
                    );

                result.Add(xmlNode);
            }

            return
                result;
        }

        IEnumerator<BaseCommand> IEnumerable<BaseCommand>.GetEnumerator()
        {
            return
                _list.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return
                _list.GetEnumerator();
        }
    }
}
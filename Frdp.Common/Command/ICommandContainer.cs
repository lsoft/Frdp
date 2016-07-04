using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Frdp.Common.Command
{
    public interface ICommandContainer
        : IEnumerable<BaseCommand>
    {
        void AddCommand(
            BaseCommand command
            );

        List<XmlNode> CreateXmlNodes(
            XmlDocument document
            );
    }
}
using System;
using System.Reflection;
using System.Security.Permissions;
using System.Xml;

namespace Frdp.Common.Command
{
    public abstract class BaseCommand
    {
        public static string AttributeName
        {
            get
            {
                return
                    "Type";
            }
        }

        public abstract XmlNode CreateXmlNode(
            XmlDocument document
            );

        public static string SalefyGetType(
            BaseCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var commandType = command.GetType();

            var type = SalefyGetType(commandType);
            
            return
                type;
        }

        public static string SalefyGetType(
            Type commandType
            )
        {
            if (commandType == null)
            {
                throw new ArgumentNullException("commandType");
            }

            var result = string.Empty;

            try
            {
                result = (string) commandType.GetProperty("CommandType", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            }
            catch
            {
                //not a command type! just ignore!
            }

            return
                result;
        }

        public abstract string ArgsToString();
    }
}

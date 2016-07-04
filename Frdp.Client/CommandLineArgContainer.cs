using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frdp.Client
{
    internal class CommandLineArgContainer
    {
        private const string IsDefaultConnectionOnlineKey = "-autoconnect";
        private const string IsSuicideTimeoutExistsKey = "-suicide_minutes";
        private const string ConnectionAddressKey = "-connect_to";

        private readonly Dictionary<string, string> _args ;

        public bool IsDefaultConnectionOnline
        {
            get
            {
                return
                    _args.ContainsKey(IsDefaultConnectionOnlineKey);
            }
        }

        public bool IsSuicideTimeoutExists
        {
            get
            {
                return
                    _args.ContainsKey(IsSuicideTimeoutExistsKey);
            }
        }

        public int SuicideTimeoutMinutes
        {
            get
            {
                int result = 0;

                if (IsSuicideTimeoutExists)
                {
                    var resultString = _args[IsSuicideTimeoutExistsKey];

                    int.TryParse(resultString, out result);
                }

                return
                    result;
            }
        }

        public bool IsConnectionAddressExists
        {
            get
            {
                return
                    _args.ContainsKey(ConnectionAddressKey);
            }
        }

        public string ConnectionAddress
        {
            get
            {
                var result = string.Empty;

                if (IsConnectionAddressExists)
                {
                    result = _args[ConnectionAddressKey];
                }

                return
                    result;
            }
        }

        public CommandLineArgContainer(
            )
        {
            _args = Environment.GetCommandLineArgs()
                .Skip(1)
                .ToDictionary(
                    j => ExtractHead(j),
                    k => ExtractTail(k),
                    StringComparer.InvariantCultureIgnoreCase
                );
        }

        private string ExtractHead(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var i = s.IndexOf(':');

            if (i <= 0)
            {
                return s;
            }

            return
                s.Substring(0, i);
        }

        private string ExtractTail(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var i = s.IndexOf(':');

            if (i <= 0)
            {
                return s;
            }

            return
                s.Substring(i + 1);
        }
    }
}

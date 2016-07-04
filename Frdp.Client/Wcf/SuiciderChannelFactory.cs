using System;
using Frdp.Client.Channel;
using Frdp.Client.Suicider;

namespace Frdp.Client.Wcf
{
    internal class SuiciderChannelFactory : IChannelFactory
    {
        private readonly IChannelFactory _channelFactory;
        private readonly IApplicationSuicider _suicider;

        public SuiciderChannelFactory(
            IChannelFactory channelFactory,
            IApplicationSuicider suicider
            )
        {
            if (channelFactory == null)
            {
                throw new ArgumentNullException("channelFactory");
            }
            if (suicider == null)
            {
                throw new ArgumentNullException("suicider");
            }

            _channelFactory = channelFactory;
            _suicider = suicider;
        }

        public IChannel OpenChannel()
        {
            var channel = _channelFactory.OpenChannel();

            //успешно создали канал
            //отключаем суицидер
            _suicider.AsyncStop();

            return
                channel;
        }
    }
}
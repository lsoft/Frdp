using System;
using Frdp.Client.Channel;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.Suicider;

namespace Frdp.Client.Wcf.MainChannel
{
    internal class SuiciderMainChannelFactory : IMainChannelFactory
    {
        private readonly IMainChannelFactory _mainChannelFactory;
        private readonly IApplicationSuicider _suicider;

        public SuiciderMainChannelFactory(
            IMainChannelFactory mainChannelFactory,
            IApplicationSuicider suicider
            )
        {
            if (mainChannelFactory == null)
            {
                throw new ArgumentNullException("mainChannelFactory");
            }
            if (suicider == null)
            {
                throw new ArgumentNullException("suicider");
            }

            _mainChannelFactory = mainChannelFactory;
            _suicider = suicider;
        }

        public IMainChannel OpenChannel()
        {
            var channel = _mainChannelFactory.OpenChannel();

            //успешно создали канал
            //отключаем суицидер
            _suicider.AsyncStop();

            return
                channel;
        }
    }
}
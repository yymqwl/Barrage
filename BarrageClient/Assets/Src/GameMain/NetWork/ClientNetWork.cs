using System;
using GameFramework;

namespace GameMain
{
    public class ClientNetWork :NetWorkBs
    {
        public ClientNetWork(NetworkProtocol networkProtocol)
        {
            m_NetworkProtocol = NetworkProtocol;
            m_ChannelType = ChannelType.Connect;
        }

    }
}

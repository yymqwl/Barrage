using GameFramework;
using System;

namespace GameMain
{
    public class ServerNetWork : NetWorkBs
    {
        public ServerNetWork(NetworkProtocol networkProtocol, string StrIpEndPoint)
        {
            m_NetworkProtocol = networkProtocol;
            m_StrIpEndPoint = StrIpEndPoint;
            m_ChannelType = ChannelType.Accept;
        }

    }
}

using GameFramework;
using System;

namespace TestWebSocket
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

using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    /// <summary>
    /// 网络相关值
    /// </summary>
    public partial class NetWorkConstant
    {
        public const int CircularBuffer_ChunkSize = 1024 * 8;
        public const string Str_Msg = "Message";

        //public const int Kcp_Delay_Time_Accept = 200;//200 ms
        public const int Kcp_Delay_Time_Connect = 300;//200 ms
        public const int Kcp_Connecting_Time = 10*1000;//10S
        public const uint KService_IdStart = 1000;
        public const uint KService_CacheLen = 1024*8;
        public const uint KcpIdleSessionTimeOut = 10*1000;



        public const int Kcp_WndSize = 256;
    }
}

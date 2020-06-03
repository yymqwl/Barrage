using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace TableRoomSilo
{
    public partial class GameConstant :TInstance<GameConstant>
    {
        public const byte TThreadInternal = 1;//10 ms刷新间隔
        /// 自动踢出玩家时间
        public const uint TPlayerIdle = 30;


        public const int ServerRate = 100;//服务器刷新率
        public const float DeltaServerTime = MathUtils.OneNub / ServerRate;//
    }
}

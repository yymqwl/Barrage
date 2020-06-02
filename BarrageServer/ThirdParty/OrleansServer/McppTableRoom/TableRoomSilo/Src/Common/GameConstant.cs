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

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace RoomServer
{
    public partial class GameConstant :TInstance<GameConstant>
    {
        public const byte TThreadInternal = 1;//10 ms刷新间隔
        /// 自动踢出玩家时间
        public const uint TPlayerIdle = 30;


        public const int MaxPlayerNub = 10000;//最大人数
        public const int RoomTotalNub = 5000;///最大房间数
        public const uint MaxRoomPlayerNub = 2;//房间最多人数
        public const uint FriendsBattlePlayerNub = 2;//好友对战人数
        public const int MaxIPercent = 100;

    }
}

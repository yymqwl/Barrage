using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public partial class GameConstant : TInstance<GameConstant>
    {
        public const string ClusterId = "dev";
        public const string ServiceId = "ChatRoomSlio";

        public const int ServerRate = 100;//服务器刷新率
        public const float DeltaServerTime = MathUtils.OneNub / ServerRate;//
    }
}

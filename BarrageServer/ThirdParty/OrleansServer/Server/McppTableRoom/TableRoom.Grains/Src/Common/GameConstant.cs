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
        public const int TClearNetUser = 60;//
        public const int TCheckNetUser = 1;
        public const int IMaxTableRoomPlayer = 2;

    }
}

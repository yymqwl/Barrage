using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace TestWebSocket
{
    public partial class GameConstant :TInstance<GameConstant>
    {
        public const byte TThreadInternal = 1;//10 ms刷新间隔

    }
}

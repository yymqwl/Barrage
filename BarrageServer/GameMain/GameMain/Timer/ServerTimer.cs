using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    public class ServerTimer : AGameTimer
    {
        public static ServerTimer Instance { get; } = new ServerTimer();
    }
}

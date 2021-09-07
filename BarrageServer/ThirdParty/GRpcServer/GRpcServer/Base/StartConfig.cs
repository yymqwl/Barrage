using GameMain.Msg;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRpcServer
{
    public class StartConfig
    {
        public List<StartConfigItem> Ls_StartConfigItem = new List<StartConfigItem>();
    }
    public class StartConfigItem
    {
        public ServerType ServerType;
        public string Rpg_Ip;
    }
}

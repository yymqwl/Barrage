using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain.Msg
{
    public enum ServerType :uint
    {
        None = 0,
        MsgParse = 1,//解析,网关
        Room = 1 << 1,//角色跟房间

        All = MsgParse | Room
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;


namespace GameMain.Msg
{

    [Message((ushort)GameMainOpCode.Ping_Msg)]
    public partial class Ping_Msg:IMessage
    {

    }
}

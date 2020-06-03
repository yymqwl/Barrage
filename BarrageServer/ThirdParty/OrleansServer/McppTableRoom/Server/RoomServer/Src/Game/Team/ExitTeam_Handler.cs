using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{

    [MessageHandler]
    public class ExitTeam_Handler : NeedLogin_Handler<ExitTeam_Req>
    {
        /*
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
           
            var exitTeam_Res = new ExitTeam_Res() { Res = -1 };
            var exitTeam_Req = message.ToObject<ExitTeam_Req>();
            var tpbv = webpy.GetIBehaviour<TeamPlayerBv>();
            exitTeam_Res.Res = GameMainEntry.Instance.TeamModule.ExitRoom(webpy);
            webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.ExitTeam_Res, exitTeam_Res));

        }*/
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var exitTeam_Res = new ExitTeam_Res() { Res = -1 };
            var exitTeam_Req = message.ToObject<ExitTeam_Req>();
            exitTeam_Res.Res = GameMainEntry.Instance.TeamModule.ExitRoom(webpy);
            webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.ExitTeam_Res, exitTeam_Res));
        }
    }
}

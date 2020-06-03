using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{

    [MessageHandler]
    public class EnterTeam_Handler : NeedLogin_Handler<EnterTeam_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var enterTeam_Res = new EnterTeam_Res();
            var enterTeam_Req = message.ToObject<EnterTeam_Req>();


            var tpbv = webpy.GetIBehaviour<TeamPlayerBv>();
            if (tpbv == null)
            {
                //Log.Debug("没有加入房间");
                tpbv = new TeamPlayerBv();
                tpbv.TeamPlayer_Data = enterTeam_Req.TeamPlayer_Data;
                webpy.AddIBehaviour(tpbv);
            }
            else
            {
                tpbv.TeamPlayer_Data = enterTeam_Req.TeamPlayer_Data;
            }

            enterTeam_Res.Res =  GameMainEntry.Instance.TeamModule.EnterTeam(webpy);
            if (enterTeam_Res.Res > 0)
            {
                var tr = GameMainEntry.Instance.TeamModule.GetTeam(tpbv.TeamId);
                enterTeam_Res.Ls_TeamPlayer_Data = tr.GetTeamPlayer_Data();
                enterTeam_Res.TeamId = tr.TeamId;
            }
            webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.EnterTeam_Res, enterTeam_Res));

        }
    }
}

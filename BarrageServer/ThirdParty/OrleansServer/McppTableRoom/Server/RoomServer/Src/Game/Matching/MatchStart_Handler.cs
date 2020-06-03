using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class MatchStart_Handler : NeedLogin_Handler<MatchStart_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var tpbv = webpy.GetIBehaviour<TeamPlayerBv>();
            if (tpbv == null)
            {
                webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.NeedTeam_Msg, new NeedEnterRoom_Msg()));
                return;
            }
            var team = GameMainEntry.Instance.TeamModule.GetTeam(tpbv.TeamId);
            if(team == null)
            {
                webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.NeedTeam_Msg, new NeedEnterRoom_Msg()));
                return;
            }
            var matchStart_Res = new MatchStart_Res() { Res =-1};
            var matchStart_Req = message.ToObject<MatchStart_Req>();
            
            if(matchStart_Req.MatchingType == (int)EMatchingType.Matching_Friend)//好友对战
            {
                if (team.Dict_Rp.Count == GameConstant.FriendsBattlePlayerNub)
                {
                    var tr = GameMainEntry.Instance.RoomModule.GetAvailableTableRoom();
                    if(tr!=null)
                    {
                        foreach(var vk in team.Dict_Rp)
                        {
                            var rpbv2 =vk.Value.GetIBehaviour<RoomPlayerBv>();
                            var tpbv2 = vk.Value.GetIBehaviour<TeamPlayerBv>();

                            tr.EnterRoom(vk.Value);
                        }
                    }
                }
                webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.MatchStart_Res , matchStart_Res));
                return;
            }

        }
    }
}

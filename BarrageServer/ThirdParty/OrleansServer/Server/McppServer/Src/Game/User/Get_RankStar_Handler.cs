using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    [MessageHandler]
    class Get_RankStar_Handler : NeedLogin_Handler<Get_RankStar_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var get_RankStar_Res = new  Get_RankStar_Res();
            get_RankStar_Res.IRank = (int)GameMainEntry.Instance.LeaderboardModule.GetRankStar_Index(webpy.Id);
            get_RankStar_Res.Ls_Leaderboard_User = GameMainEntry.Instance.LeaderboardModule.Leaderboard_Stars;
            var bys = Msg_Json.Create_Msg_Json(NetOpCode.Get_RankStar_Res, get_RankStar_Res);
            webpy.SendAsync(bys);
        }
    }
}

using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class GetNextFrame_Handler : NeedInRoom_Handler<GetNextFrame_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            if(tr.RoomState == ERoomState.ERoom_InGame ||
                tr.RoomState == ERoomState.ERoom_GameOver)
            {
                GetNextFrame_Req getNextFrame_Req = message.ToObject<GetNextFrame_Req>();
                GetNextFrame_Res getNextFrame_Res = new GetNextFrame_Res();
                var tmp_fms = tr.ServerPlayer.PlayerRecoder.GetCurToEndFrame(getNextFrame_Req.CurFrameId);
                foreach(var fm in tmp_fms)
                {
                    getNextFrame_Res.ListFrame.Add(fm);
                }
                webpy.SendAsync(Msg_Json.Create_Msg_Json( NetOpCode.GetNextFrame_Res, getNextFrame_Res));
            }
        }
    }
}

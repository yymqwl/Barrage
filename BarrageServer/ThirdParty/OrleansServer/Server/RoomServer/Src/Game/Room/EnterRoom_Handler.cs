using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class EnterRoom_Handler : NeedLogin_Handler<EnterRoom_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var enterRoom_Res = new EnterRoom_Res();
            var enterRoom_Req = message.ToObject<EnterRoom_Req>();

            enterRoom_Res.Res = GameMainEntry.Instance.SiloClientModule.ITableRoomEntry.Join(enterRoom_Req.TableUser_Data).Result;
            if(enterRoom_Res.Res>0)//加入房间成功
            {
                enterRoom_Res.TableRoomInfo =  GameMainEntry.Instance.SiloClientModule.ITableRoomEntry.GetUserTableRoomInfo(enterRoom_Req.TableUser_Data.Id).Result;
            }
            webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.EnterRoom_Res, enterRoom_Res));
            /*
            var rpbv= webpy.GetIBehaviour<RoomPlayerBv>();
            if(rpbv == null)
            {
                //Log.Debug("没有加入房间");
                rpbv = new RoomPlayerBv();
                rpbv.RoomPlayer_Data = enterRoom_Req.RoomPlayer_Data;
                webpy.AddIBehaviour(rpbv);
            }
            else
            {
                rpbv.RoomPlayer_Data = enterRoom_Req.RoomPlayer_Data;
            }

            enterRoom_Res.Res = GameMainEntry.Instance.RoomModule.EnterRoom(webpy);
            if (enterRoom_Res.Res > 0)
            {
                var tr = GameMainEntry.Instance.RoomModule.GetTableRoom(rpbv.RoomId);
                enterRoom_Res.Ls_RoomPlayer_Data = tr.GetRoomPlayer_Data();
            }
            */


        }
    }
}

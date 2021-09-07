using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace RoomServer
{

    [MessageHandler]
    public class Login_Handler : AMWHandler<Login_Req>
    {
        protected override void Run(IWebSocketSession session, JObject message)
        {
            var login_req = message.ToObject<Login_Req>();
            var py = GameMainEntry.Instance.PlayerModule.GetPlayer(login_req.Id);
            if (py == null)
            {
                py = GameMainEntry.Instance.PlayerModule.CreatePlayer(login_req.Id);
            }
            Log.Debug($"Py{py.Id}:登录成功");
            py.CheckNetUser().Wait();
            GameMainEntry.Instance.PlayerModule.SetSession_Py(py, session);
            py.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.Login_Res, new Login_Res { Res = 1 }));
        }
    }
}

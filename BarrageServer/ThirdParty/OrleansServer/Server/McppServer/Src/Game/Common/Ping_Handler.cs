using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;
using GameFramework;

namespace Mcpp
{

    [MessageHandler]
    public class Ping_Handler : AMWHandler<Ping_Msg>
    {
        protected override void Run(IWebSocketSession session, JObject message)
        {
            var bys = Msg_Json.Create_Msg_Json(NetOpCode.Ping_Msg, message);
            session.Context.WebSocket.Send(bys);

            var py = GameMainEntry.Instance.PlayerModule.Session_Pys.GetValueByKey(session.ID);
            if (py != null)
            {
                py.LastActiveTime = DateTime.Now;
            }
        }
    }
}

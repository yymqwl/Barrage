using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Mcpp
{
    //微信处理
    [MessageHandler]
    public class WeChat_Login_Handler : AMWHandler<WeChat_Login_Req>
    {

        protected override void Run(IWebSocketSession session, JObject message)
        {
            Task.Factory.StartNew(() =>
            {
                var login_req = message.ToObject<WeChat_Login_Req>();
                var str_requst = WeChatHelper.GetLogin(login_req.Code);
                Log.Debug("Requst:"+str_requst);
                var res = HttpHelper.CreateHttp(str_requst);
                var jsres = JObject.Parse(res);
                //jsres[""]
                //Log.Debug("res:"+res);
                WeChat_Login_Res weChat_Login_Res = new WeChat_Login_Res();
                weChat_Login_Res.Id = jsres["openid"].Value<string>();
                var bys = Msg_Json.Create_Msg_Json(NetOpCode.WeChat_Login_Res, weChat_Login_Res);
                session.Context.WebSocket.Send(bys);
            });
        }
    }
}

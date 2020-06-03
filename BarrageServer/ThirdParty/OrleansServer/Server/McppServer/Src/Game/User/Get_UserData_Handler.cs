using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    [MessageHandler]
    public class Get_UserData_Handler : NeedLogin_Handler<Get_UserData_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var bys = Msg_Json.Create_Msg_Json(NetOpCode.Get_UserData_Res, webpy.User_Data);
            webpy.SendAsync(bys);
        }
    }
}

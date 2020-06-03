using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Mcpp
{
    [MessageHandler]
    public class Set_UserData_Handler : NeedLogin_Handler<Set_UserData_Req>
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {

            foreach(var item in message)
            {
                webpy.User_Data[item.Key] = item.Value;

                if(item.Key == GameConstant.Str_Stars)
                {
                    GameMainEntry.Instance.LeaderboardModule.RecordStars(webpy);
                }
                //先简单处理 GameMainEntry.Instance.DataObserverModule.DataEventManager.FireNow(webpy, UserDataChange_Args.Create(item));
            }
            webpy.SaveData();
            //重新给所有
        }
    }
}

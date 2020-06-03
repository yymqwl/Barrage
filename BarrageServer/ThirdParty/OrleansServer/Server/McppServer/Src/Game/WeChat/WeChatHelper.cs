using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    public static  class WeChatHelper
    {
        public static string GetLogin(string strcode)
        {
            var setting = GameMainEntry.Instance.SettingModule.ServerSetting;

            return $"https://api.weixin.qq.com/sns/jscode2session?appid={setting.WeChat_AppId}&secret={setting.WeChat_AppSecret}&js_code={strcode}&grant_type=authorization_code";
        }
    }
}

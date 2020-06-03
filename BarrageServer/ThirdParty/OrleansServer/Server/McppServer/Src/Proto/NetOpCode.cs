using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    public enum NetOpCode : ushort
    {
        Ping_Msg = 100,

        Login_Req = 200,//登录
        Login_Res,
        NeedLogin_Msg,//需要登录

        Get_UserData_Req = 300,//获取用户数据
        Get_UserData_Res,
        Set_UserData_Req,
        Set_UserData_Res,
        Get_RankStar_Req,//获取排行榜星
        Get_RankStar_Res,

        WeChat_Login_Req = 1000,//微信登录
        WeChat_Login_Res,//



        EnterTeam_Req=3000,
        EnterTeam_Res,
        ExitTeam_Req,
        ExitTeam_Res,
        TeamPlayerJoin_Msg,
        TeamPlayerLeave_Msg,
        TeamDisband_Msg,//队伍解散
        MatchStart_Req,
        MatchStart_Res,
        NeedTeam_Msg,
    }
}

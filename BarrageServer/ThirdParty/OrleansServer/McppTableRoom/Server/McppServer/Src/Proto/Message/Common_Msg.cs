using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{


    
    [Message((ushort)NetOpCode.Ping_Msg)]
    public class Ping_Msg : IMessage
    {
        public long Time;
    }
    [Message((ushort)NetOpCode.Login_Req)]
    public class Login_Req:IMessage
    {
        public string Id;
    }
    [Message((ushort)NetOpCode.Login_Res)]
    public class Login_Res : IMessage
    {
        public int Res;
    }
    [Message((ushort)NetOpCode.NeedLogin_Msg)]
    public class NeedLogin_Msg : IMessage
    {

    }
    [Message((ushort)NetOpCode.Get_RankStar_Req)]
    public class Get_RankStar_Req : IMessage
    {

    }
    [Message((ushort)NetOpCode.Get_RankStar_Res)]
    public class Get_RankStar_Res : IMessage
    {
        public int IRank;//排名
        public List<Leaderboard_User> Ls_Leaderboard_User;
    }

    [Message((ushort)NetOpCode.Get_UserData_Req)]
    public class Get_UserData_Req : IMessage
    {

    }
    [Message((ushort)NetOpCode.Get_UserData_Res)]
    public class Get_UserData_Res : IMessage
    {

    }

    [Message((ushort)NetOpCode.Set_UserData_Req)]
    public class Set_UserData_Req : IMessage
    {

    }
    [Message((ushort)NetOpCode.Set_UserData_Res)]
    public class Set_UserData_Res : IMessage
    {

    }
    [Message((ushort)NetOpCode.WeChat_Login_Req)]
    public class WeChat_Login_Req : IMessage
    {
        public string Code;
    }
    [Message((ushort)NetOpCode.WeChat_Login_Res)]
    public class WeChat_Login_Res:IMessage
    {
        public string Id;
    }






    /*
    [Message((ushort)NetOpCode.EnterTeam_Req)]
    public class EnterTeam_Req : IMessage
    {
        public TeamPlayer_Data TeamPlayer_Data;
    }
    [Message((ushort)NetOpCode.EnterTeam_Res)]
    public class EnterTeam_Res : IMessage
    {
        public int Res;
        public string TeamId;//
        public List<TeamPlayer_Data> Ls_TeamPlayer_Data;
    }
    [Message((ushort)NetOpCode.TeamPlayerJoin_Msg)]
    public class TeamPlayerJoin_Msg : IMessage
    {
        public TeamPlayer_Data TeamPlayer_Data;
    }
    [Message((ushort)NetOpCode.TeamPlayerLeave_Msg)]
    public class TeamPlayerLeave_Msg : IMessage
    {
        public TeamPlayer_Data TeamPlayer_Data;
    }
    
    [Message((ushort)NetOpCode.ExitTeam_Req)]
    public class ExitTeam_Req : IMessage
    {
    }
    [Message((ushort)NetOpCode.ExitTeam_Res)]
    public class ExitTeam_Res : IMessage
    {
        public int Res;
    }
    [Message((ushort)NetOpCode.NeedTeam_Msg)]
    public class NeedTeam_Msg : IMessage
    {

    }
    [Message((ushort)NetOpCode.TeamDisband_Msg)]
    public class TeamDisband_Msg : IMessage
    {

    }*/
}

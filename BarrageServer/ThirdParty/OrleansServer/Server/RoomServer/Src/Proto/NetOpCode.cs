using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public enum NetOpCode : ushort
    {
        Ping_Msg = 100,

/// <summary>
/// 登录阻隔
/// </summary>
        Login_Req = 200,//登录
        Login_Res,
        NeedLogin_Msg,//需要登录

/////////////////////////房间协议
        EnterRoom_Req=2000,
        EnterRoom_Res,
        NeedEnterRoom_Msg,//
        RoomPlayerJoin_Msg,//房间消息
        RoomPlayerLeave_Msg,
        ExitRoom_Req,
        ExitRoom_Res,

        EnterTeam_Req,
        EnterTeam_Res,
        ExitTeam_Req,
        ExitTeam_Res,
        TeamPlayerJoin_Msg,
        TeamPlayerLeave_Msg,
        TeamDisband_Msg,//队伍解散
        MatchStart_Req,
        MatchStart_Res,
        NeedTeam_Msg,

        EnterChatRoom_Req = 2500,
        EnterChatRoom_Res,
        ExitChatRoom_Req,
        ExitChatRoom_Res,
        ChatPlayerJoin_Msg,
        ChatPlayerLeave_Msg,
        GetChatRoom_Detail_Req,
        GetChatRoom_Detail_Res,

        //////////////////////房间内部协议
        RoomReady_Req =3000,//准备操作结果
        RoomReady_Res,//准备操作结果
        RoomReadyFinish_Req,
        RoomReadyFinish_Res,
        RoomLoading_Req,//载入
        RoomLoading_Res,//载入结束
        GameStart_Req,
        GameStart_Res,
        GameOver_Req,
        GameOver_Res,
        ////////////////////LockStep消息
        Command_Msg ,
        GetNextFrame_Req,
        GetNextFrame_Res,
    }
}

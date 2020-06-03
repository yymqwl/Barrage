using GameFramework;
using GameMain.LockStep;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RoomServer
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
    public class TeamPlayerJoin_Msg :IMessage
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


    [Message((ushort)NetOpCode.MatchStart_Req)]
    public class MatchStart_Req : IMessage
    {
        public int MatchingType;
    }
    [Message((ushort)NetOpCode.MatchStart_Res)]
    public class MatchStart_Res : IMessage
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
        
    }



    [Message((ushort)NetOpCode.NeedLogin_Msg)]
    public class NeedLogin_Msg : IMessage
    {

    }

    [Message((ushort)NetOpCode.EnterRoom_Req)]
    public class EnterRoom_Req:IMessage
    {
        public RoomPlayer_Data RoomPlayer_Data;
    }

    [Message((ushort)NetOpCode.EnterRoom_Res)]
    public class EnterRoom_Res : IMessage
    {
        public int Res;
        public List<RoomPlayer_Data> Ls_RoomPlayer_Data;
    }

    [Message((ushort)NetOpCode.RoomPlayerJoin_Msg)]
    public class RoomPlayerJoin_Msg : IMessage
    {
        public RoomPlayer_Data RoomPlayer_Data;
    }
    [Message((ushort)NetOpCode.RoomPlayerLeave_Msg)]
    public class RoomPlayerLeave_Msg : IMessage
    {
        public RoomPlayer_Data RoomPlayer_Data;
    }
    [Message((ushort)NetOpCode.NeedEnterRoom_Msg)]
    public class NeedEnterRoom_Msg : IMessage
    {

    }


    [Message((ushort)NetOpCode.ExitRoom_Req)]
    public class ExitRoom_Req : IMessage
    {
    }
    [Message((ushort)NetOpCode.ExitRoom_Res)]
    public class ExitRoom_Res : IMessage
    {
        public int Res;
    }
    [Message((ushort)NetOpCode.RoomReady_Req)]
    public class RoomReady_Req : IMessage
    {
        public bool IsReady;
        public JObject StartGame_Data;
    }

    [Message((ushort)NetOpCode.RoomReadyFinish_Req)]
    public class RoomReadyFinish_Req : IMessage
    {
        public List<JObject> Ls_StartGame_Data=new List<JObject>();
    }
    [Message((ushort)NetOpCode.RoomReadyFinish_Res)]
    public class RoomReadyFinish_Res : IMessage
    {

    }

    [Message((ushort)NetOpCode.RoomReady_Res)]
    public class RoomReady_Res : IMessage
    {
        public string Id;
        public bool IsReady;
    }
    [Message((ushort)NetOpCode.RoomLoading_Req)]
    public class RoomLoading_Req : IMessage
    {
        public int IPercent;
    }

    [Message((ushort)NetOpCode.RoomLoading_Res)]
    public class RoomLoading_Res : IMessage
    {
        public string Id;
        public int IPercent;
    }

    [Message((ushort)NetOpCode.GameStart_Req)]
    public class GameStart_Req : IMessage
    {

    }

    [Message((ushort)NetOpCode.GameStart_Res)]
    public class GameStart_Res : IMessage
    {

    }
    [Message((ushort)NetOpCode.GameOver_Req)]
    public class GameOver_Req : IMessage
    {

    }
    [Message((ushort)NetOpCode.GameOver_Res)]
    public class GameOver_Res : IMessage
    {
       
    }

    [Message((ushort)NetOpCode.Command_Msg)]
    public  class Command_Msg : IMessage
    {
        public Command Command;
    }
    [Message((ushort)NetOpCode.GetNextFrame_Req)]
    public class GetNextFrame_Req : IMessage
    {
        public uint CurFrameId;
    }

    [Message((ushort)NetOpCode.GetNextFrame_Res)]
    public class GetNextFrame_Res : IMessage
    {
        public List<Frame_Data> ListFrame = new List<Frame_Data>();
    }

}

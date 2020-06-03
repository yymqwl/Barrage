using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain.LockStep
{

    public class Command : IMessage
    {
        public string CommandSender;//发送者
        public int CommandType;//操作类型
        public float Data_1;
        public float Data_2;
        public float Data_3;
    }
    public class Frame_Data : IMessage
    {
        public uint FrameId;//编号
        public List<Command> Commands = new List<Command>();
    }
    public class StartGame_Data:IMessage
    {

    }
    public class ReplayData:IMessage
    {
        public StartGame_Data StartGame_Data;
        public List<Frame_Data> ListFrame;
        public uint EndFrameId;//编号
    }

    public interface ISendCommand
    {
        void SendCommand(Command cmd);
    }
    public interface IHandleFrame
    {
        void HandleFrame(Frame_Data frame);
    }
    public interface IHandleCommand
    {
        void HandleCommand(Command cmd);
    }

}

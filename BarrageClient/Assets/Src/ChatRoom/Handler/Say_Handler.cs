using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.ChatRoom;
namespace ChatRoom
{
    [MessageHandler]
    public class Say_Handler : AMHandler<Say_Res>
    {
        protected override void Run(Session session, Say_Res message)
        {

            ChatRoomUI.Instance.m_Sb.AppendLine(message.Msg);
            //Log.Debug($"{message.Msg}");
        }
    }
}

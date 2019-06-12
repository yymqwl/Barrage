using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.ChatRoom;
using IHall;
namespace BarrageSilo
{
    [MessageHandler]
    public class Login_Handler : AMHandler<Login_Req>
    {
        protected async override void Run(Session session, Login_Req message)
        {
            if(session.GetIBehaviour<UserIdBv>() !=null)
            {
                Log.Debug("Has Logined");
                return;
            }

            var client = GameModuleManager.Instance.GetModule<SiloClient>();


            var ime = client.MainEntry;
            var chatroom = await ime.GetIChatRoom();
            
            var chatuser = await chatroom.EnterRoom(message.Id);

            await chatuser.SetSessionId(session.Id);
            await chatuser.SetName(message.Id.ToString());

            var useridbv =  new UserIdBv(message.Id);
            session.AddIBehaviour(useridbv);

            session.Send(new Login_Res());
        }
    }
}

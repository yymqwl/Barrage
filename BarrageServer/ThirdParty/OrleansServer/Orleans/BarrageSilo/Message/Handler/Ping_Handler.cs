using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using IHall;
using GameMain.ChatRoom;


namespace BarrageSilo
{
    [MessageHandler]
    public class Ping_Handler : AMHandler<Ping_Msg>
    {
        protected async override void Run(Session session, Ping_Msg message)
        {
            var uidbv  = session.GetIBehaviour<UserIdBv>();
            if(uidbv==null)
            {
                session.Send(message);
                return;
            }
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var user = await client.ChatRoom.GetChatUser(uidbv.Id);

            await user.Ping();

            session.Send(message);
        }
    }
}

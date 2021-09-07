using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using GameMain.ChatRoom;
using IHall;

namespace BarrageSilo
{
    //[MessageHandler]
    public class ExitRoom_Handler : AMHandler<ExitRoom_Req>
    {
        protected async override void Run(Session session, ExitRoom_Req message)
        {
            var uidbv = session.GetIBehaviour<UserIdBv>();

            
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            await client.ChatRoom.ExitRoom(uidbv.Id);
            /*
            var ime = client.ClusterClient.GetGrain<IMainEntry>(0);
            var ihello = await ime.GetIHello();
            var msg = await ihello.SayHello();
            */
            //session.Send(message);
        }
    }
}

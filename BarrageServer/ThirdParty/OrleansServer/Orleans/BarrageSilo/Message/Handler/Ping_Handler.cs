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
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var ime = client.ClusterClient.GetGrain<IMainEntry>(0);
            var ihello  = await ime.GetIHello();
            var msg = await ihello.SayHello();
            Log.Debug(msg);

            session.Send(message);
        }
    }
}

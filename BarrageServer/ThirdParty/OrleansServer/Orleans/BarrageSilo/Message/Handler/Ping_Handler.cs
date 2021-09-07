using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using IHall;
using GameMain.ChatRoom;


namespace BarrageSilo
{
    //[MessageHandler]
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

            var dt1 = DateTime.UtcNow;

            await client.ChatRoom.DirectPingUser(uidbv.Id);//合并操作

            var ts = DateTime.UtcNow - dt1;
            Log.Debug($"InnerPing1:{ts.TotalMilliseconds}");

            dt1 = DateTime.UtcNow;
            var user = await client.ChatRoom.GetChatUser(uidbv.Id);
            ts = DateTime.UtcNow - dt1;
            Log.Debug($"InnerPing2:{ts.TotalMilliseconds}");

            dt1 = DateTime.UtcNow;
            await user.Ping();
            ts = DateTime.UtcNow - dt1;
            Log.Debug($"InnerPing3:{ts.TotalMilliseconds}");



            session.Send(message);
        }
    }
}

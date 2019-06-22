using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GameFramework;
using GameMain.ChatRoom;
using IHall;
namespace BarrageSilo
{
    [MessageHandler]
    public class Say_Handler : AMHandler<Say_Req>
    {
        protected  async override void Run(Session session, Say_Req message)
        {
            var useridbv = session.GetIBehaviour<UserIdBv>();
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var ichatuser = await client.ChatRoom.GetChatUser(useridbv.Id);

            Log.Debug($"Say:ThreadId:{Thread.CurrentThread.ManagedThreadId}");

            await ichatuser.Say(message.Msg);
            Log.Debug("sayfinish");
        }
    }
}

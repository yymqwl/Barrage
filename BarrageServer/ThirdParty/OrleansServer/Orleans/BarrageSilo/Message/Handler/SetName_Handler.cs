using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using GameMain.ChatRoom;
using IHall;
namespace BarrageSilo
{
    [MessageHandler]
    public class SetName_Handler : AMHandler<SetName_Req>
    {
        protected async override void Run(Session session, SetName_Req message)
        {

            var useridbv = session.GetIBehaviour<UserIdBv>();
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var ichatuser = await client.ChatRoom.GetChatUser(useridbv.Id);

            await ichatuser.SetName(message.Name);

        }
    }
}

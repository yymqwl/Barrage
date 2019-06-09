using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.Msg;
using IHall;
namespace BarrageSilo
{
    [MessageHandler]
    public class Say_Handler : AMHandler<Say_Req>
    {
        protected override void Run(Session session, Say_Req message)
        {
            var useridbv = session.GetIBehaviour<UserIdBv>();
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var ichatuser = client.ClusterClient.GetGrain<IChatUser>(useridbv.Id);

            ichatuser.Say(message.Msg);

        }
    }
}

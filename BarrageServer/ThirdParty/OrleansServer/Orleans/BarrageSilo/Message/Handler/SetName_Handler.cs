using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using GameMain.Msg;
using IHall;
namespace BarrageSilo
{
    [MessageHandler]
    public class SetName_Handler : AMHandler<SetName_Req>
    {
        protected override void Run(Session session, SetName_Req message)
        {

            var useridbv = session.GetIBehaviour<UserIdBv>();
            var client = GameModuleManager.Instance.GetModule<SiloClient>();
            var ichatuser = client.ClusterClient.GetGrain<IChatUser>(useridbv.Id);

            ichatuser.SetName(message.Name);

        }
    }
}

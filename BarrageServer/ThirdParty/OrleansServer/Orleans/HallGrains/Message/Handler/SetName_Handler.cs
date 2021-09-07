using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameMain.ChatRoom;
using IHall;
using Orleans;

namespace HallGrains
{
    [MessageHandler]
    public class SetName_Handler : ARpcCall<SetName_Req, SetName_Res>
    {
        public async override Task<SetName_Res> Run(long userid, SetName_Req requst, IGrainFactory grainfactory)
        {
            var meg = grainfactory.GetGrain<IMainEntry>(0);
            var icr = await meg.GetIChatRoom();
            var ichatuser = await icr.GetChatUser(userid);
            if(ichatuser == null)
            {
                return  null;
            }
            await ichatuser.SetName(requst.Name);
            return new SetName_Res() { Result = 1 };
        }
    }
}

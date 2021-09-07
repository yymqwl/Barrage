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
    public class Say_Handler : ARpcCall<Say_Req, Say_Res>
    {
        public async override Task<Say_Res> Run(long userid, Say_Req request, IGrainFactory grainfactory)
        {
            var meg = grainfactory.GetGrain<IMainEntry>(0);
            var icr = await meg.GetIChatRoom();
            var ichatuser = await icr.GetChatUser(userid);
            if (ichatuser == null)
            {
                return null;
            }
            await ichatuser.Say(request.Msg);

            return null;
            //return new Say_Res() { Msg = request.Msg };
        }
    }
}

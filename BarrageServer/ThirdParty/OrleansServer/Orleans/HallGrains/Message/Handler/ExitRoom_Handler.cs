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
    public class ExitRoom_Handler : ARpcCall<ExitRoom_Req, ExitRoom_Res>
    {
        public async override Task<ExitRoom_Res> Run(long userid, ExitRoom_Req request, IGrainFactory grainfactory)
        {
            var meg = grainfactory.GetGrain<IMainEntry>(0);
            var icr = await meg.GetIChatRoom();
            await icr.ExitRoom(userid);

            return null;
            //return new ExitRoom_Res() { Result = 1 };
        }
    }
}

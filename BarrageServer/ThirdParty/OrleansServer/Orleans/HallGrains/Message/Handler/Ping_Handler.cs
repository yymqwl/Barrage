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
    public class Ping_Handler : ARpcCall<Ping_Msg, Ping_Msg>
    {
        public async  override Task<Ping_Msg> Run(long userid, Ping_Msg request, IGrainFactory grainfactory)
        {
            var meg = grainfactory.GetGrain<IMainEntry>(0);
            var dt1 = DateTime.UtcNow;

            var icr = await meg.GetIChatRoom();
            var user = await icr.GetChatUser(userid);
            if(user == null)
            {
                return request;
            }
            await user.Ping();
            var ts = DateTime.UtcNow - dt1;
            Log.Debug($"InnerPing1:{ts.TotalMilliseconds}");

            dt1 = DateTime.UtcNow;
            await icr.DirectPingUser(userid);//合并操作
            ts = DateTime.UtcNow - dt1;
            Log.Debug($"InnerPing2:{ts.TotalMilliseconds}");

            return request;
        }
    }
}

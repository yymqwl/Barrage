using Orleans;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{

    //一个网关只能有一个消息回调
    public interface IGateWay: IGrainWithIntegerKey
    {
        Task SubscribeAsync(IGateWay_Obs view);

        Task UnSubscribeAsync(IGateWay_Obs view);
        Task Ping(IGateWay_Obs view);


        Task<byte[]> Call(long id,byte[] msg);

        Task Reply(long id,byte[] msg);
    }
    /*
     *         Task<bool> Ping();
    public class GateWayItem
    {
        public IGateWay_Obs GateWay_Viewer { get; set; }
        public long LastActiveTime { get; set; }
    }*/
}

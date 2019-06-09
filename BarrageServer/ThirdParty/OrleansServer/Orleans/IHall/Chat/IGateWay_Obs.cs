using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace IHall
{
    /// <summary>
    /// 玩家消息
    /// </summary>
    public interface IGateWay_Obs : IGrainObserver
    {
        void Reply(long Id, IMessage msg);
    }
}

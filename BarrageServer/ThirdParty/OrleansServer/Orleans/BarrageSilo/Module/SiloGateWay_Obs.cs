using GameFramework;
using IHall;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarrageSilo
{
    public class SiloGateWay_Obs : IGateWay_Obs
    {
        public void Reply(long Id, IMessage msg)
        {
            var silonw = GameModuleManager.Instance.GetModule<SiloNetWork>();
            var session = silonw.ServerNetWork.Get(Id);
            if(session == null)
            {
                Log.Debug("已经断开Session");
            }
            else
            {
                session.Send(msg);
            }
        }
    }
}

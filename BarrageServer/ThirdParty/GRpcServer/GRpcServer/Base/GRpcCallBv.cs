using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace GRpcServer
{
    public class GRpcCallBv: MessageDispatherBv
    {
        public override int Priority => 10;
        public override bool Init()
        {
            return base.Init();
        }

        public override void Handle(Session session, MessageInfo messageInfo)
        {
            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}

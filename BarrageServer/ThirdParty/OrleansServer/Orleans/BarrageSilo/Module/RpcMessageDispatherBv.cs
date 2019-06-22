using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;


namespace BarrageSilo
{
    public class RpcMessageDispatherBv : MessageDispatherBv
    {
        public async override void Handle(Session session, MessageInfo messageInfo)
        {
            List<IMHandler> actions;
            if (!m_Dict_Handlers.TryGetValue(messageInfo.Opcode, out actions))
            {
                var useridbv = session.GetIBehaviour<UserIdBv>();
                if(useridbv==null)
                {
                    return;
                }

                var client = GameModuleManager.Instance.GetModule<SiloClient>();
                IMessage message = await client.GateWay.Call(useridbv.Id, messageInfo.Message);
                session.Send(message);
                return;
            }
            else
            {
                try
                {
                    foreach (IMHandler ev in actions)
                    {
                        ev.Handle(session, messageInfo.Message);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }


        }
    }
}

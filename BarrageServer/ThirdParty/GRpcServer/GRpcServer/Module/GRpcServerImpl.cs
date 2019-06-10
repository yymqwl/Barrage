using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameMain.Msg;
using Grpc.Core;

namespace GRpcServer
{
    public class GRpcServerImpl: GameMain.Msg.GRpcServer.GRpcServerBase
    {
        public override Task<GRpcMsg> Handle(GRpcMsg request, ServerCallContext context)
        {
            return base.Handle(request, context);
        }
    }
}

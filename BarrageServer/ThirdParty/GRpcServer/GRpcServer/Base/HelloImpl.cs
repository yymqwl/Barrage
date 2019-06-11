using GameMain.Msg;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GRpcServer
{
    public class HelloImpl : GameMain.Msg.Hello.HelloBase
    {
        public override Task<Hello_Msg> Handle(Hello_Msg request, ServerCallContext context)
        {
            return Task.FromResult(new Hello_Msg { Msg = $"Server:{request.Msg}" });
        }
        public override Task<Hello_Ping> Ping(Hello_Ping request, ServerCallContext context)
        {
            return Task.FromResult(request);
        }
    }
}

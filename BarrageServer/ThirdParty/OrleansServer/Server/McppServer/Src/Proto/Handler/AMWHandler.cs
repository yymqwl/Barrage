using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace Mcpp
{
    public abstract class AMWHandler<T> : IMWHandler where T:class,IMessage
    {
        public Type GetMessageType()
        {
            return typeof(T);
        }
        protected abstract void Run(IWebSocketSession session, JObject message);
        public void Handle(IWebSocketSession session, JObject message)
        {
            if (message == null || session == null)
            {
                Log.Error($"消息类型转换错误:JObject");
                return;
            }
            this.Run(session, message);
        }
    }
}

using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace Mcpp
{
    
    public interface IMWHandler
    {
        void Handle(IWebSocketSession session, JObject message);
        Type GetMessageType();

    }
}

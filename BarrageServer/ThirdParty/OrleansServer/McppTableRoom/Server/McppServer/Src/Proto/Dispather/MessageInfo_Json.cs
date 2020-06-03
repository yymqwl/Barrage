using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    public struct MessageInfo_Json
    {
        public ushort Opcode { get; }
        public JObject Message { get; }

        public MessageInfo_Json(ushort opcode, JObject message)
        {
            this.Opcode = opcode;
            this.Message = message;
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Newtonsoft.Json;

namespace RoomServer
{
    public static class Msg_Json
    {
        public static byte[] Create_Msg_Json(NetOpCode id,JObject data)
        {
            JObject msg_json = new JObject();
            msg_json["Id"] = (ushort)id;
            msg_json["Data"] = data;
            var str_msg =  JsonConvert.SerializeObject(msg_json);
            return LZString.CompressToUint8Array(str_msg);

        }
        public static byte[] Create_Msg_Json<T>(NetOpCode id, T msg)where T:IMessage
        {
            JObject msg_json = new JObject();
            msg_json["Id"] = (ushort)id;
            msg_json["Data"] = JObject.FromObject(msg);
            var str_msg = JsonConvert.SerializeObject(msg_json);
            return LZString.CompressToUint8Array(str_msg);
        }
    }
}

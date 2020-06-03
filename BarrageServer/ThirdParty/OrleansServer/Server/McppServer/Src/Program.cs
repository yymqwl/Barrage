using System;
using GameFramework;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Mcpp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var jsobj = new JObject();
            /*
             var jsobj = JObject.FromObject(new Ping_Msg() { Time = 55555 });
            Ping_Msg ping_Msg = jsobj.ToObject<Ping_Msg>();
            */
            //Log.Debug(JsonConvert.SerializeObject(jsobj) );
            //Console.WriteLine(TimeHelper.ClientNow());
            GameMainEntry.Instance.Entry(args);
        }
    }
}

/*
  //var jsobj = new JObject();


  //jsobj.Add("nihao", 222);
  //jsobj["nihao"] = 222;
  //jsobj.Add("nihao",3333);

  //Log.Debug(JsonConvert.SerializeObject(jsobj));
  */

using GameMain;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using IHall;
namespace SlioClient
{
    public class ClientConsoleModule : ConsoleModule
    {
        protected SiloModule SiloModule
        {
            get
            {
                return GameModuleManager.Instance.GetModule<SiloModule>();
            }
        }
        public override void Parse(Console_Command cmd)
        {
            base.Parse(cmd);
            switch(cmd.CommandType)
            {
                case "hello":
                    var gateway=  SiloModule.ClusterClient.GetGrain<IGateWay>(0);
                    //byte[] bys =  gateway.Call(0,Encoding.Default.GetBytes("Client")).Result;

                    break;
                default:break;
            }
        }
    }
}

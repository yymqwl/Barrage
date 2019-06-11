using GameMain;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using IHall;
using GameMain.ChatRoom;
using System.IO;

namespace SlioClient
{
    public class ClientConsoleModule : ConsoleModule
    {
        public override bool Init()
        {
            /*
            MemoryStream ms = new MemoryStream();

            ms.Seek(2,SeekOrigin.Begin);
            Pb_3Helper.Serialize(new Login_Req() { Id=10}, ms);
            ms.Seek(2, SeekOrigin.Begin);
            var obj =  Pb_3Helper.Deserialize( typeof(Login_Req) ,ms);
            */

            return base.Init();
        }
        protected SiloModule SiloModule
        {
            get
            {
                return GameModuleManager.Instance.GetModule<SiloModule>();
            }
        }
        protected SiloNetWork SiloNetWork
        {
            get
            {
                return GameModuleManager.Instance.GetModule<SiloNetWork>();
            }
        }
        protected Session m_Session;
        public  override void Parse(Console_Command cmd)
        {
            base.Parse(cmd);
            switch(cmd.CommandType)
            {
                case "login":
                    Log.Debug($"Id{cmd.Params[0]}");
                    if(m_Session==null || !m_Session.IsConnected)
                    {
                        m_Session = SiloNetWork.ServerNetWork.Create("127.0.0.1:2000");
                        
                    }
                    else
                    {
                        var login_req = new Login_Req();
                        login_req.Id = long.Parse(cmd.Params[0]);
                        m_Session.Send(login_req);
                    }
                    break;
                case "ping":
                    {
                        var ping_msg = new Ping_Msg();
                        ping_msg.Time = DateTime.UtcNow.Ticks;
                        m_Session.Send(ping_msg);
                    }
                    break;
                case "say":
                    {
                        var say_req = new Say_Req();
                        say_req.Msg = cmd.Params[0];
                        m_Session.Send(say_req);
                    }
                    break;
                case "name":
                    {
                        var set_name = new SetName_Req();
                        set_name.Name = cmd.Params[0];
                        m_Session.Send(set_name);
                    }
                    break;
                default:break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace WebServer
{
    [GameFrameworkModule()]
    public class HttpServerModule : GameFrameworkModule
    {
        public override int Priority => 1000;
        protected HttpServer m_HttpServer;
        protected ServerSetting m_ServerSetting;
        public override bool Init()
        {
            Log.Debug("HttpServerModule Init");

            var xmldata = FileHelper.GetDataFromFile("Setting/ServerSetting.xml");
            m_ServerSetting = XmlHelper.XmlDeserialize<ServerSetting>(xmldata);

            m_HttpServer = new HttpServer();

            m_HttpServer.Start(m_ServerSetting.Login_Ip);

            //var str= XmlHelper.XmlSerialize_Str(new ServerSetting() { Login_Ip="111"});
            return base.Init();
        }

        public override bool ShutDown()
        {
            Log.Debug("HttpServerModule ShutDown");
            m_HttpServer.Stop();
            return base.ShutDown();
        }

        public override void Update()
        {
        }
    }

}

using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Mcpp
{
    [GameFrameworkModule()]
    public class WebServerModule : GameFrameworkModule
    {
        public override int Priority => 0;
        
        protected WebSocketServer m_WebSocketServer;
        protected OpCodeTypeBv m_OpCodeTypeBv;
        public OpCodeTypeBv OpCodeTypeBv
        {
            get
            {
                return this.m_OpCodeTypeBv;
            }
        }
        protected WMessageDispather m_Dispather;
        public WMessageDispather Dispather
        {
            get
            {
                return this.m_Dispather;
            }
        }
        RootBehaviour<WebServerModule> m_Root;
        public override bool Init()
        {
            m_Root = new RootBehaviour<WebServerModule>(this);
            m_OpCodeTypeBv = new OpCodeTypeBv();
            m_Root.AddIBehaviour(m_OpCodeTypeBv);
            m_OpCodeTypeBv.Load(typeof(WebServerModule).Assembly);


            m_Dispather = new WMessageDispather();
            m_Root.AddIBehaviour(m_Dispather);
            m_Dispather.Load(typeof(WebServerModule).Assembly);


            
            m_WebSocketServer = new WebSocketServer(GameMainEntry.Instance.SettingModule.ServerSetting.Web_Ip);
            m_WebSocketServer.KeepClean = true;
            {
                m_WebSocketServer.SslConfiguration.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("Setting/www.hengtaiyunyou.com.pfx", "AEQEvO54");
            }
            //m_WebSocketServer.Log.Level = LogLevel.Debug;
            m_WebSocketServer.AddWebSocketService<WebBv>("/");
            m_WebSocketServer.Start();


            Log.Debug("WebSocketServer Start ");
            m_Root.Init();

            return base.Init();
        }
        public override bool ShutDown()
        {
            m_WebSocketServer.Stop();
            m_Root.ShutDown();
            Log.Debug("WebSocketServer Stop");
            return base.ShutDown();
        }
        public override void Update()
        {
            
        }
    }
}

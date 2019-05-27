using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework;
namespace GameMain
{

    //[GameFrameworkModule()]
    public class TestKcpMd : GameFrameworkModule
    {
        KService m_KService;

        public override bool Init()
        {
            IdGenerater.AppId = 10;
            m_KService = new KService(NetHelper.ToIPEndPoint("127.0.0.1", 2000), Accept);
            m_KService.DisConnectedCallback += Service_DisConnectCallback;
            Log.Debug("TestKcpMd");
            return base.Init();
        }

        private void Service_DisConnectCallback(AChannel ac)
        {
            Log.Debug($"{ac.Id}:disconnect");
        }

        private void Ac_ReadCallback(AChannel ac,MemoryStream ms)
        {
            Log.Debug(Encoding.UTF8.GetString(ms.ToArray()));
        }

        public void Accept(AChannel  ac)
        {
            ac.ReadCallback += Ac_ReadCallback;
            Log.Debug($"{ac.Id}:Accept");
        }
        public override void Update()
        {
            m_KService.Update();
            //KService.Instance.Update();
        }

        public override bool Shutdown()
        {
            m_KService.Dispose();
            //KService.Instance.Dispose();
            return base.Shutdown();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace GameMain
{

    [GameFrameworkModule()]
    public class TestKcpMd : GameFrameworkModule
    {

        public override bool Init()
        {
            KService kService = new KService(NetHelper.ToIPEndPoint("127.0.0.1", 2000), Accept);
            Log.Debug("TestKcpMd");
            return base.Init();
        }
        public void Accept(AChannel  ac)
        {
            Log.Debug($"{ac.Id}:Accept");
        }
        public override void Update()
        {
            KService.Instance.Update();
        }

        public override bool Shutdown()
        {
            KService.Instance.Dispose();
            return base.Shutdown();
        }
    }
}
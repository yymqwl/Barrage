using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace GameMain
{

    [GameFrameworkModule()]
    public class TestAsyncMd : GameFrameworkModule
    {
        public override bool Init()
        {
            Log.Debug($"{ TimeHelper.ClientNowSeconds()}");
            return base.Init();
        }
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}

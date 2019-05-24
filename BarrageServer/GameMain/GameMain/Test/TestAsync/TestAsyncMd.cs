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
            //Log.Debug($"{ TimeHelper.ClientNowSeconds()}");
            return base.Init();
        }
        public override void Update()
        {
            //Log.Debug($"{ ServerTimer.Instance.DeltaTime}");
        }
    }
}

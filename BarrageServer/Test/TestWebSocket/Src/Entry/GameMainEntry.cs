using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestWebSocket
{
    public class GameMainEntry : AGameMainEntry
    {

        public static GameMainEntry Instance { get; } = new GameMainEntry();
        protected override void Init()
        {
            GameModuleManager.Instance.CreateModules(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.Init();
        }
        protected  override void ShutDown()
        {
            GameModuleManager.Instance.ShutDown();
        }
      
    }
}

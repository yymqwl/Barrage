using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    public class TestGameEntry : GameMainEntry
    {

        public static TestGameEntry Instance { get; } = new TestGameEntry();
        protected override void Init()
        {
            m_GameModuleManager = new GameModuleManager();
            m_GameModuleManager.CreateModules(typeof(TestGameEntry).Assembly);

            m_GameModuleManager.Init();

        }
        protected override void ShutDown()
        {
            m_GameModuleManager.Shutdown();
            base.ShutDown();
        }

    }
}

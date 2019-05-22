using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    public class TestGameEntry : GameMainEntry
    {
        protected override void Init()
        {

            m_GameModuleManager = new GameModuleManager();
            m_GameModuleManager.CreateModules(typeof(TestGameEntry).Assembly);
            m_GameModuleManager.Init();

        }

    }
}

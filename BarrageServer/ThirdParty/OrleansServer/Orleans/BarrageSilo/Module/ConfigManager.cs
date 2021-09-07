using GameFramework;
using GameMain.Msg;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarrageSilo
{
    [GameFrameworkModuleAttribute]
    public class ConfigManager : GameFrameworkModule
    {



        public override int Priority => -1000;

        public override bool Init()
        {

            var cfgdata = FileHelper.GetDataFromFile("./Config/ServerConfig.xml");

            m_GameConfig = XmlHelper.XmlDeserialize<ServerConfig>(cfgdata);

            return base.Init();
        }
        public ServerConfig GameConfig
        {
            get
            {
                return m_GameConfig;
            }
        }
        public ServerConfig m_GameConfig;

        public override void Update()
        {

        }
    }
}

using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRpcServer
{
    [GameFrameworkModuleAttribute]
    public class ConfigManager : GameFrameworkModule
    {


        
        public override int Priority => -1000;

        public override bool Init()
        {

            var cfgdata = FileHelper.GetDataFromFile("./Config/StartCfg.xml");

            m_StartConfig = XmlHelper.XmlDeserialize<StartConfig>(cfgdata);
            /*
            m_StartConfig = new StartConfig();
            m_StartConfig.ServerType = ServerType.All;

            var item = new StartConfigItem();
            item.Rpg_Ip = "127.0.0.1:5000";
            item.ServerType = ServerType.None;
            m_StartConfig.Ls_StartConfigItem.Add(item);

            string str_xml = XmlHelper.XmlSerialize_Str(m_StartConfig);
            */

            return base.Init();
        }
        public StartConfig StartConfig
        {
            get
            {
                return m_StartConfig;
            }
        }
        public StartConfig m_StartConfig;


        public StartConfigItem GetStartConfigItem(ServerType serverType)
        {
           return  m_StartConfig.Ls_StartConfigItem.Find((StartConfigItem item)=>
            {
                if(item.ServerType == serverType)
                {
                    return true;
                }
                return false;
            }); 
        }
        public override void Update()
        {

        }
    }
}

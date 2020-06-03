using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace RoomServer
{
    [GameFrameworkModule]
    public class SettingModule : GameFrameworkModule
    {
        public override int Priority => -100;
        public ServerSetting ServerSetting
        {
            get { return m_ServerSetting; }
        }
        protected ServerSetting m_ServerSetting;

        public override bool Init()
        {
            var server_data = FileHelper.GetDataFromFile("Setting/ServerSetting.xml");

            this.m_ServerSetting  = XmlHelper.XmlDeserialize<ServerSetting>(server_data);



            return base.Init();
        }
        public override void Update()
        {
        }
    }
}

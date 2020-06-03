using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RoomServer
{
    public class GameMainEntry : AGameMainEntry
    {

        public static GameMainEntry Instance { get; } = new GameMainEntry();

        protected SettingModule m_SettingModule;
        public SettingModule SettingModule 
        {
            get 
            {
                if(m_SettingModule==null)
                {
                    m_SettingModule =  GameModuleManager.Instance.GetModule<SettingModule>();
                }
                return m_SettingModule;
            }
        }
        protected WebServerModule m_WebServerModule;
        public WebServerModule WebServerModule
        {
            get
            {
                if (m_WebServerModule == null)
                {
                    m_WebServerModule = GameModuleManager.Instance.GetModule<WebServerModule>();
                }
                return m_WebServerModule;
            }
        }
        protected RoomModule m_RoomModule;
        public RoomModule RoomModule
        {
            get
            {
                if(this.m_RoomModule == null)
                {
                    this.m_RoomModule = GameModuleManager.Instance.GetModule<RoomModule>();
                }
                return this.m_RoomModule;
            }
        }
        
        protected TeamModule m_TeamModule;
        public TeamModule TeamModule
        {
            get
            {
                if (m_TeamModule == null)
                {
                    m_TeamModule = GameModuleManager.Instance.GetModule<TeamModule>();
                }
                return m_TeamModule;
            }
        }

        protected PlayerModule m_PlayerModule;
        public PlayerModule PlayerModule
        {
            get
            {
                if(this.m_PlayerModule == null)
                {
                    m_PlayerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
                }
                return this.m_PlayerModule;
            }
        }
        protected SiloClientModule m_SiloClientModule;

        public SiloClientModule SiloClientModule
        {
            get
            {
                if (this.m_SiloClientModule == null)
                {
                    m_SiloClientModule = GameModuleManager.Instance.GetModule<SiloClientModule>();
                }
                return this.m_SiloClientModule;
            }
        }



        protected override void Init()
        {
            AssemblyManager.Instance.Add(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.CreateModules(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.Init();
        }
        protected  override void ShutDown()
        {
            GameModuleManager.Instance.ShutDown();
        }
      
    }
}

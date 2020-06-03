using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mcpp
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
        protected DbModule m_DbModule;
        public DbModule DbModule
        {
            get
            {
                if (this.m_DbModule == null)
                {
                    m_DbModule = GameModuleManager.Instance.GetModule<DbModule>();
                }
                return this.m_DbModule;
            }
        }
        protected DataObserverModule m_DataObserverModule;
        public DataObserverModule DataObserverModule
        {
            get
            {
                if (this.m_DataObserverModule == null)
                {
                    m_DataObserverModule = GameModuleManager.Instance.GetModule<DataObserverModule>();
                }
                return this.m_DataObserverModule;
            }
        }
        protected LeaderboardModule m_LeaderboardModule;
        public LeaderboardModule LeaderboardModule
        {
            get
            {
                if(this.m_LeaderboardModule == null)
                {
                    m_LeaderboardModule = GameModuleManager.Instance.GetModule<LeaderboardModule>();
                }
                return this.m_LeaderboardModule;
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

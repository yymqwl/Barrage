using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameFramework;
using System.Threading;

namespace GameMain
{
    public class GameMainEntry : UInstance<GameMainEntry>, IGameMainEntry
    {
        public IGameModuleManager GameModuleManager => m_GameModuleManager;

        protected IGameModuleManager m_GameModuleManager;
        
        public void Entry(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            ClientTimer.Instance.Start();
            m_GameModuleManager =  new GameModuleManager();
            m_GameModuleManager.CreateModules(typeof(GameMainEntry).Assembly);
            m_GameModuleManager.Init();
            

        }
        
        private void Awake()
        {
            Entry(null);
        }

        public void OnApplicationPause(bool pause)
        {
            Log.Debug($"OnApplicationPause{pause}");
        }
        public void OnApplicationQuit()
        {
            m_GameModuleManager.Shutdown();
            m_GameModuleManager = null;
            Log.Debug($"OnApplicationQuit");
        }

        private void Update()
        {
            try
            {
                ClientTimer.Instance.Update();
                m_GameModuleManager.Update();
                OneThreadSynchronizationContext.Instance.Update();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }
    }
}

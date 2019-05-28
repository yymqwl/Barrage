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
        //public IGameModuleManager GameModuleManager => GameModuleManager.Instance;

        //protected IGameModuleManager m_GameModuleManager;
        
        public void Entry(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            ClientTimer.Instance.Start();
            GameModuleManager.Instance.CreateModules(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.Init();
            

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
            GameModuleManager.Instance.Shutdown();
            Log.Debug($"OnApplicationQuit");
        }

        private void Update()
        {
            try
            {
                ClientTimer.Instance.Update();
                GameModuleManager.Instance.Update();
                OneThreadSynchronizationContext.Instance.Update();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }
    }
}

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
    public class AGameMainEntry : MonoBehaviour, IGameMainEntry
    {
        public virtual void Entry(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            ClientTimer.Instance.Start();
            //GameModuleManager.Instance.CreateModules(typeof(GameMainEntry).Assembly);
            //GameModuleManager.Instance.Init();
            

        }
        
        private  void Awake()
        {
            Entry(null);
        }

        public virtual void OnApplicationPause(bool pause)
        {
            Log.Debug($"OnApplicationPause{pause}");
        }
        public virtual void OnApplicationQuit()
        {
            GameModuleManager.Instance.ShutDown();
            Log.Debug($"OnApplicationQuit");
        }

        public virtual void Update()
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

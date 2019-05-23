﻿using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameMain
{
    public class GameMainEntry : IGameMainEntry
    {
        public IGameModuleManager GameModuleManager => m_GameModuleManager;

        protected bool m_IsLoop;
        public bool IsLoop { get => m_IsLoop; set { m_IsLoop = value; } }

        protected IGameModuleManager m_GameModuleManager;

        protected virtual void Init()
        {

        }
        protected virtual void ShutDown()
        {

        }

        public virtual void Main(string[] args)
        {

            m_IsLoop = true;
            ///所有消息拉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
            {

                Init();
                ServerTimer.Instance.Start();
                while (m_IsLoop)
                {
                    try
                    {
                        Thread.Sleep(GameConstant.TThreadInternal);
                        ServerTimer.Instance.Update();
                        m_GameModuleManager.Update();
                        OneThreadSynchronizationContext.Instance.Update();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
                ShutDown();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
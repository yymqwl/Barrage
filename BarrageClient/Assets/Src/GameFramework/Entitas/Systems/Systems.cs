using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class Systems : IShutDownSystem,IExecuteSystem,IInitializeSystem
    {
        System_Collection<IShutDownSystem> m_IShutDownSystems=new System_Collection<IShutDownSystem>();
        System_Collection<IExecuteSystem> m_IExecuteSystems = new System_Collection<IExecuteSystem>();
        System_Collection<IInitializeSystem> m_IInitializeSystems = new System_Collection<IInitializeSystem>();

        public int Priority => 0;





        public virtual T AddSystem<T>(T  sys) where T:ISystem
        {
            var initializeSystem = sys as IInitializeSystem;
            if (initializeSystem != null)
            {
                m_IInitializeSystems.AddSystem(initializeSystem);
                return sys;
            }
            var executeSystem = sys as IExecuteSystem;
            if (executeSystem != null)
            {
                m_IExecuteSystems.AddSystem(executeSystem);
                return sys;
            }

            var shutdownSystem = sys as IShutDownSystem;
            if (shutdownSystem != null)
            {
                m_IShutDownSystems.AddSystem(shutdownSystem);
                return sys;
            }

            return default(T);
        }


        public virtual T GetSystem<T>() where T : class,ISystem
        {
            T tmpsys = default(T);
            foreach (var sys in m_IExecuteSystems.Systems)
            {
                tmpsys = sys as T;
                if(tmpsys!=null)
                {
                    return tmpsys;
                }
            }
            return default(T);
        }

        public virtual void Execute(float elapseSeconds)
        {
            m_IExecuteSystems.Exect_Order((IExecuteSystem sys) =>
            {
                sys.Execute(elapseSeconds);
            });
        }

        public virtual bool Initialize()
        {
            //激活响应Sys
            m_IExecuteSystems.Exect_Order((IExecuteSystem sys) =>
            {
                IReactiveSystem irs = sys as IReactiveSystem;
                if(irs!=null)
                {
                    irs.Activate();
                }
            });


            m_IInitializeSystems.Exect_ReverseOrder((IInitializeSystem sys) =>
            {
                sys.Initialize();
            });
            return true;
        }

        public virtual bool ShutDown()
        {
            m_IShutDownSystems.Exect_ReverseOrder((IShutDownSystem sys) =>
            {
                sys.ShutDown();
            });

            //清理Sys
            m_IExecuteSystems.Exect_Order((IExecuteSystem sys) =>
            {
                IReactiveSystem irs = sys as IReactiveSystem;
                if (irs != null)
                {
                    irs.Deactivate();
                    irs.Clear();
                }
            });

           
            m_IInitializeSystems.Systems.Clear();
            m_IExecuteSystems.Systems.Clear();
            m_IShutDownSystems.Systems.Clear();


            return true;
        }
    }
}

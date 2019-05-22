using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameMain
{
    /// <summary>
    /// 游戏框架入口
    /// </summary>
    public class GameModuleManager : IGameModuleManager
    {
        public  string GameModuleVersion => "1.1.0";
        protected  LinkedList<GameFrameworkModule> m_GameFrameworkModules = new LinkedList<GameFrameworkModule>();
        public  LinkedList<GameFrameworkModule> GameFrameworkModules
        {
            get
            {
                return m_GameFrameworkModules;
            }
        }


        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public  void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (GameFrameworkModule module in m_GameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }
        public  void Init()
        {
            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.BeforeInit();
            }

            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Init();
            }

            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.AfterInit();
            }

        }

        public  void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.BeforeShutdown();
            }

            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            for (LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.AfterShutdown();
            }

            m_GameFrameworkModules.Clear();
        }
        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        public  T GetModule<T>() where T : class
        {

            Type interfaceType = typeof(T);
            return GetModule(interfaceType) as T;
        }
        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        public  GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in m_GameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }
            throw new GameFrameworkException($"Cant Find Type{moduleType.Name}");
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private  GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(string.Format("Can not create module '{0}'.", module.GetType().FullName));
            }

            LinkedListNode<GameFrameworkModule> current = m_GameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_GameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                m_GameFrameworkModules.AddLast(module);
            }

            return module;
        }
        public  void CreateModules(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                object[] objects = type.GetCustomAttributes(typeof(GameFrameworkModuleAttribute), false);
                if (objects.Length == 0)
                {
                    continue;
                }
                CreateModule(type);
            }
        }



    }
}

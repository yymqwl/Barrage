using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameFramework
{
    public interface IGameModuleManager
    {
        string GameModuleVersion { get; }
        LinkedList<GameFrameworkModule> GameFrameworkModules { get; }
        void Update();
        void Init();
        void Shutdown();
        T GetModule<T>() where T : GameFrameworkModule;
        GameFrameworkModule GetModule(Type moduleType);
        void CreateModules(Assembly assembly);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    /// <summary>
    /// 类似于Unity 组件系统
    /// </summary>
    public interface IBehaviourSet :IBehaviour
    {
        T GetIBehaviour<T>() where T : class,IBehaviour;
        IBehaviour GetIBehaviour(Type tp);
        T[] GetIBehaviours<T>() where T : class, IBehaviour;
        IBehaviour[] GetIBehaviours(Type tp);

        T AddIBehaviour<T>(T t) where T : class, IBehaviour;

        void RemoveIBehaviour<T>(T ib) where T : class, IBehaviour;
    }
}

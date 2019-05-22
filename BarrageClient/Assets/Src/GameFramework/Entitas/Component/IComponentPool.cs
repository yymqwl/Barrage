using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class IComponentPool : CObjectPool<IComponent>
    {
        public static IComponentPool Instance { get; } = new IComponentPool();

    }

    
}

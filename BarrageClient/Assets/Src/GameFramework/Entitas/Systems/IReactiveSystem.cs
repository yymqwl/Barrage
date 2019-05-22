using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public interface IReactiveSystem: IExecuteSystem
    {
        void Activate();
        void Deactivate();
        void Clear();
    }
}

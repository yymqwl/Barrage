using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public interface  IExecuteSystem :ISystem
    {
        void Execute(float elapseSeconds);
    }
}

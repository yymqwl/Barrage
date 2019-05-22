using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public interface IShutDownSystem : ISystem
    {
         bool ShutDown();
    }
}

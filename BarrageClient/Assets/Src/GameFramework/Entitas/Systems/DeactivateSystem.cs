using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class DeactivateSystem : IShutDownSystem
    {
        public int Priority => 1000;
       


        public bool ShutDown()
        {
            Log.Debug("DeactivateSystem");
            return true;
        }
    }
}

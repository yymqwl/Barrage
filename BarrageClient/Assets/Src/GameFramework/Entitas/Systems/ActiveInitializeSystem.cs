using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public class ActiveInitializeSystem:IInitializeSystem
    {
        public int Priority => -1000;

        public bool Initialize()
        {
            Log.Debug("ActiveInitializeSystem");


            return true;
        }

    }
}

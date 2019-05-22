using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public  interface IInitializeSystem: ISystem
    {
          bool Initialize();
    }
}

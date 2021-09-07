using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    interface IRecycle
    {
        void OnSpawn();


        void OnUnspawn();

        bool Init();


        void Release();

    }
}

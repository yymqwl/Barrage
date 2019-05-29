using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public interface IBehaviour
    {
        int Priority { get; }
        IBehaviour Parent { get; set; }

        T GetParent<T>() where T : class, IBehaviour;
        bool Init();

        bool ShutDown();

        void Update();

    }
}

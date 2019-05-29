using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMain
{
    [GameFrameworkModule]
    public class ConfigModule : GameFrameworkModule
    {
        public override int Priority => -10000;

        public override bool Init()
        {
            AssemblyManager.Instance.Add(GetType().Assembly);
            return base.Init();
        }
        public override void Update()
        {

        }
    }
}

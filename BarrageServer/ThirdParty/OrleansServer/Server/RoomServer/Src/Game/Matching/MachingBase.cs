using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace RoomServer
{
    public class MachingBase : ABehaviour
    {
        protected readonly uint TMatching_Active_Time = 15;//15s空闲删除时间
        protected uint TMatching_AI_Time = 3;//30;匹配AI时间

        public override bool Init()
        {
            return base.Init();
        }
        public override bool ShutDown()
        {

            return base.ShutDown();
        }


    }
}

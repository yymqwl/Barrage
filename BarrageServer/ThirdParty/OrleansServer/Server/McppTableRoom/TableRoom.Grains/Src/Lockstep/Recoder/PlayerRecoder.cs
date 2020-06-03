using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMain;
using GameFramework;

namespace GameMain.LockStep
{
    /// <summary>
    /// 游戏录制器
    /// </summary>
    public class PlayerRecoder: FrameRecoder
    {

        protected override void ResetFrame(int capacity)
        {
            base.ResetFrame(capacity);
            m_End_FrameId = 0;
        }

        public override bool Init()
        {
            ResetFrame(GameConstant.RecoderCapacity);
            return base.Init();
        }

    }
}

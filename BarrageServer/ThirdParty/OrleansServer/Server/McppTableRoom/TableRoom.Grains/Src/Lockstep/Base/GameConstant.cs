using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMain
{
    public partial class GameConstant : TInstance<GameConstant>
    {

        public const uint IAsyncWaitFrame = 1;//客户端载入等待的帧

        public readonly uint TTcpCheckPing = 5; //1S检查一次Ping
        public readonly uint TKcpCheckPing = 1; //1S检查一次Ping

        public const uint TServerCheckOutOfTime = 1;

        //////////////////////////////////////////////////////////////////////////
        public const uint DefaultMsgPoolSize = 1024;
        public const uint DefaultMsgPoolIncrease = 1024;



        public static readonly int DefaultSteamBuffer = 512;




        public const uint ServerFrameRate = 30;//服务器发送帧率

        public const uint FrameTolerate = 20;//最多容忍
        public const uint Play_Mul2_Trg = 10;//2倍速度播放追赶
        public const uint Play_Mul4_Trg = 15;//

        public const uint Play_Mul_1 = 1;//1倍速度
        public const uint Play_Mul_2 = 2;//2倍速度
        public const uint Play_Mul_4 = 4;//4倍速度


        public const uint FrameRate = 45;//逻辑帧率
        public const float DeltaTimeF = MathUtils.OneNub / FrameRate;//逻辑刷新帧率

        public const float DeltaServerTimeF = MathUtils.OneNub / ServerFrameRate;//请求服务器频率


        public const int RecoderCapacity = 60 * 5 * (int)FrameRate;

        /// Frame容器
        public const uint NFrameInitCount = FrameRate * 5 * 60;//初始2分钟帧
        public const uint NFrameExtendCount = FrameRate * 1 * 60;//每次扩展1分钟帧


        public const float TouchDeltaTimeF = MathUtils.OneNub / ServerFrameRate;//1s 中10次
        public const float TouchLength = 0f;
        //////////////////////////////////////////////////////////////////////////
        ///


        public const uint TResourcesCleanUp = 60;//资源清理
        public const uint TResourcesTiming = 1;//资源刷新


    }
}

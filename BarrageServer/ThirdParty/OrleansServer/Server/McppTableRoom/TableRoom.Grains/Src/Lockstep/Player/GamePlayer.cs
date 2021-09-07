using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;


namespace GameMain.LockStep
{
    public class GamePlayer: PlayerBase ,ISendCommand
    {
        public enum EPlaySpeedMode
        {
            E_Mul_1,//1倍速度
            E_Mul_2,//2倍速度
            E_Mul_4,//4倍速度
            E_CatchUp,//追赶播放
        }

        protected EPlaySpeedMode m_PlaySpeedMode;
        public EPlaySpeedMode PlaySpeedMode
        {
            get
            {
                return m_PlaySpeedMode;
            }
        }
        protected UpdateTime m_Update_Frame;

        protected PlayerInput CreatePlayerInput()
        {

            return new PlayerInput();
        }


        public virtual bool Load(StartGame_Data startGame_Data)
        {

            m_PlayerRecoder.Load(startGame_Data);
            return true;
        }
        public override bool Init()
        {
            m_PlaySpeedMode = EPlaySpeedMode.E_Mul_1;
            m_Update_Frame = new UpdateTime(GameConstant.DeltaTimeF);
            m_Update_Frame.Evt_Act += Update_Frame;

            return base.Init();
        }
        public override bool ShutDown()
        {
            this.m_Update_Frame.Dispose();
            return base.ShutDown();
        }
        public override void Play()
        {
            base.Play();
        }
        public override void Stop()
        {
            base.Stop();
        }
        /// <summary>
        /// 正常刷新
        /// </summary>
        /// <param name="elapseSeconds"></param>
        protected override void Update(float elapseSeconds)
        {
        }
        protected override void HandleFrame(Frame_Data frame)
        {
        }
        protected override void GameFrameTurn(float elapseSeconds)
        {
            if (FrameCount >= m_PlayerRecoder.GetEndFrameId())
            {
                return;
            }
            base.GameFrameTurn(elapseSeconds);
        }
        protected void GameFrameTurn_Mul(uint imul)
        {
            for (uint i = 0; i < imul; ++i)
            {
                GameFrameTurn(GameConstant.DeltaTimeF);
            }
        }
        public override void Update()
        {
            //////////////////////////////////////////////////////////////////////////
            ///*********************游戏播放器
            if (m_IsStop)
            {
                return;
            }
            //////////////////////////////////////////////////////////////////////////
            ////一秒播放60个逻辑帧,收到新的逻辑帧就刷新
            ///
            ////触发追赶

            if ((FrameCount + GameConstant.FrameTolerate) < m_PlayerRecoder.GetEndFrameId())
            {
                m_PlaySpeedMode = EPlaySpeedMode.E_CatchUp;
            }
            else if ((FrameCount + GameConstant.Play_Mul4_Trg) < m_PlayerRecoder.GetEndFrameId())
            {
                m_PlaySpeedMode = EPlaySpeedMode.E_Mul_4;
            }
            else if ((FrameCount + GameConstant.Play_Mul2_Trg) < m_PlayerRecoder.GetEndFrameId())
            {
                m_PlaySpeedMode = EPlaySpeedMode.E_Mul_2;
            }
            else
            {
                m_PlaySpeedMode = EPlaySpeedMode.E_Mul_1;
            }


            if (PlaySpeedMode == EPlaySpeedMode.E_Mul_1)
            {
                GameFrameTurn(GameConstant.DeltaTimeF);
            }
            else if (PlaySpeedMode == EPlaySpeedMode.E_Mul_2)
            {
                Log.Debug("2加速-FrameCount:" + FrameCount + "---GetEndFrameId:" + m_PlayerRecoder.GetEndFrameId());
                GameFrameTurn_Mul(GameConstant.Play_Mul_2);
            }
            else if (PlaySpeedMode == EPlaySpeedMode.E_Mul_4)
            {
                Log.Debug("4加速-FrameCount:" + FrameCount + "---GetEndFrameId:" + m_PlayerRecoder.GetEndFrameId());
                GameFrameTurn_Mul(GameConstant.Play_Mul_4);
            }
            else //触发追赶播放
            {
                Log.Debug("追赶加速-FrameCount:" + FrameCount + "---GetEndFrameId:" + m_PlayerRecoder.GetEndFrameId());
                while (FrameCount < m_PlayerRecoder.GetEndFrameId())
                {
                    GameFrameTurn(GameConstant.DeltaTimeF);
                }
            }

        }
        protected void Update_Frame()
        {

        }
        public virtual void SendCommand(Command cmd)
        {

        }
    }
}

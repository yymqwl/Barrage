using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using GameFramework;
using GameFramework.Fsm;
using GameMain.LockStep;
using Microsoft.Extensions.ObjectPool;

namespace RoomServer
{


    public class TableRoom : ARoom
    {

        protected ERoomState m_RoomState = ERoomState.ERoom_Idle;

        public ERoomState RoomState
        {
            get { return m_RoomState; }
            set { this.m_RoomState = value; }
        }

        Fsm<TableRoom> m_Fsm;
        public Fsm<TableRoom> Fsm
        {
            get { return m_Fsm; }
        }

        ServerPlayer m_ServerPlayer;
        public ServerPlayer ServerPlayer
        {
            get { return m_ServerPlayer; }
        }


        public TableRoom()
        {

        }


        public override bool Init(string roomid)
        {
            var pret = base.Init(roomid);


            var tr_idle = new TR_Idle();
            var tr_ready = new TR_Ready();
            var tr_loading= new TR_Loading();
            var tr_ingame = new TR_InGame();
            var tr_gameover = new TR_GameOver();
            m_Fsm = new Fsm<TableRoom>(this.GetType().FullName, this,
                tr_idle, tr_ready, tr_loading, tr_ingame, tr_gameover);
            this.Fsm.Start<TR_Idle>();

            /////////////////////
            return pret;
        }
        public override bool ShutDown()
        {
            ShutDownServerPlayer();
            return base.ShutDown();
        }
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            this.m_Fsm.Update(elapseSeconds, realElapseSeconds);
            this.CheckInActivePy();
        }
        public void InitServerPlayer()
        {
            ShutDownServerPlayer();
            m_ServerPlayer = new ServerPlayer();
            m_ServerPlayer.Init();
            m_ServerPlayer.Play();
        }
        public void ShutDownServerPlayer()
        {
            if(m_ServerPlayer!=null)
            {
                m_ServerPlayer.ShutDown();
                m_ServerPlayer = null;
            }
        }
        public void ResetPlayer_Ready()
        {
            foreach (var rp in m_Dict_Rp)
            {
                var rpd = rp.Value.GetIBehaviour<RoomPlayerBv>();
                rpd.RestReady();
            }
        }

        public void CheckAllLoading()
        {
            if (this.Dict_Rp.Count != GameConstant.MaxRoomPlayerNub)
            {
                return;
            }
            bool ballloading = true;
            foreach (var rp in m_Dict_Rp)
            {
                var rpd = rp.Value.GetIBehaviour<RoomPlayerBv>();
                if (rpd.RoomPlayer_Data.IPercent != GameConstant.MaxIPercent)
                {
                    ballloading = false;
                    break;
                }
            }
            if (ballloading)
            {
                this.Fsm.ChangeState<TR_InGame>();
            }
        }
        /// <summary>
        /// 检查准备就绪
        /// </summary>
        public void CheckAllReady()
        {
            if (this.Dict_Rp.Count != GameConstant.MaxRoomPlayerNub)
            {
                return;
            }
            bool ballready = true;
            foreach (var rp in m_Dict_Rp)
            {
                var rpd = rp.Value.GetIBehaviour<RoomPlayerBv>();
                if (!rpd.RoomPlayer_Data.IsReady)
                {
                    ballready = false;
                    break;
                }
            }
            if (ballready)
            {
                this.SendReadyFinish();
                this.Fsm.ChangeState<TR_InGame>();
            }
        }
        public void CheckAllGameOver()
        {
            bool ballgameover = true;
            foreach (var rp in m_Dict_Rp)
            {
                var rpd = rp.Value.GetIBehaviour<RoomPlayerBv>();
                if (rpd.RoomPlayerState != ERoomPlayerState.E_GameOver)
                {
                    ballgameover = false;
                    break;
                }
            }
            if (ballgameover)
            {
                Log.Debug("CheckAllGameOver GameOverFinish");
                this.GameOverFinish();
            }
        }
        public void GameOverFinish()
        {
            ShutDownServerPlayer();
            ResetPlayer_Ready();
            Fsm.ChangeState<TR_Ready>();
        }

        public void ChangeToGameOver()
        {
            if(this.Fsm.IsInState<TR_InGame>())
            {
                this.Fsm.ChangeState<TR_GameOver>();
            }
        }
        protected void SendReadyFinish()
        {
            var readyfinish_req = new RoomReadyFinish_Req();
            foreach (var rp in m_Dict_Rp)
            {
                var rpd = rp.Value.GetIBehaviour<RoomPlayerBv>();
                readyfinish_req.Ls_StartGame_Data.Add(rpd.StartGame_Data);
                rpd.RoomPlayerState = ERoomPlayerState.E_InGame;
            }
            this.SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.RoomReadyFinish_Req, readyfinish_req));
        }

        public override void OpenRoom()
        {
            base.OpenRoom();
            this.m_Fsm.ChangeState<TR_Ready>();
        }
        public override void CloseRoom()
        {
            base.CloseRoom();
            this.Fsm.ChangeState<TR_Idle>();
        }
        public List<RoomPlayer_Data> GetRoomPlayer_Data()
        {
            var Ls_RoomPlayer_Data = new List<RoomPlayer_Data>();
            foreach(var vk in this.Dict_Rp)
            {
                var rpb= vk.Value.GetIBehaviour<RoomPlayerBv>();
                if(rpb !=null && rpb.RoomPlayer_Data !=null)
                {
                    Ls_RoomPlayer_Data.Add(rpb.RoomPlayer_Data);
                }
            }
            return Ls_RoomPlayer_Data;
        }


    }

    public enum ERoomState
    {
        ERoom_Idle,//空闲没人状态
        ERoom_Ready,//有人正在准备组队
        ERoom_Loading,//所有人准备就绪准备开始游戏
        ERoom_InGame,//进入游戏
        ERoom_GameOver,//游戏结束返回Ready状态
    }
}

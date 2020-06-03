using GameFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace RoomServer
{

    [GameFrameworkModule]
    public class RoomModule : GameFrameworkModule
    {

        public override int Priority
        {
            get
            {
                return 1000;
            }
        }

        protected Dictionary<string,TableRoom> m_Dict_Room = new Dictionary<string,TableRoom>();


        public override bool Init()
        {
            CreateRoom();


            return base.Init();
        }
        public TableRoom GetTableRoom(string id)
        {
            TableRoom tr;
            m_Dict_Room.TryGetValue(id, out tr);
            return tr;
        }

        public TableRoom GetCanJoinTableRoom()
        {
            foreach (var vk in m_Dict_Room)
            {
                if(!vk.Value.IsAvailable
                    && vk.Value.Dict_Rp.Count < GameConstant.MaxRoomPlayerNub)
                {
                    return vk.Value;
                }
            }
            return null;

        }

        public TableRoom GetAvailableTableRoom()
        {
            foreach (var vk in m_Dict_Room)
            {
                if (vk.Value.IsAvailable)
                {
                    vk.Value.IsAvailable = false;
                    return vk.Value;
                }
            }
            return null;
        }

        protected void CreateRoom()
        {
            this.m_Dict_Room.Clear();
            for(int i=0;i<GameConstant.RoomTotalNub;++i)
            {
                var tr = new TableRoom();
                tr.Init(i.ToString());//房间号
                this.m_Dict_Room.Add(i.ToString(), tr);
            }
        }

        /// <summary>
        /// -1房间不够
        /// -2必须先创建房间
        /// </summary>
        /// <param name="webPlayer"></param>
        /// <returns></returns>
        public int EnterRoom(WebPlayer webPlayer)
        {
            var ret = -1;
            var rpv = webPlayer.GetIBehaviour<RoomPlayerBv>();
            if(rpv == null)
            {
                return ret;
            }

            TableRoom tr = null;
            if(rpv.RoomId != string.Empty)
            {
                tr = this.GetTableRoom(rpv.RoomId);
                if(tr.IsAvailable)//没人必须创建房间
                {
                    ret = -2;
                    return ret;
                }
                ret = tr.EnterRoom(webPlayer);
                return ret;
            }

            if(rpv.RoomPlayer_Data.RoomId == string.Empty)//创建房间
            {
                ///////////////////////////这部分用于测试,自动匹配有人的房间
                tr = this.GetCanJoinTableRoom();
                if(tr !=null)
                {
                    ret = tr.EnterRoom(webPlayer);
                    return ret;
                }
                /////////////////////////////////////////////////
                //创建新房间
                tr = this.GetAvailableTableRoom();
                if (null == tr)
                {
                    return ret;
                }




                tr.OpenRoom();//
                ret = tr.EnterRoom(webPlayer);
                rpv.RoomId = tr.RoomId;
            }
            else//加入别的房间
            {
                tr = this.GetTableRoom(rpv.RoomPlayer_Data.RoomId );
                if (tr.IsAvailable)//没人必须创建房间
                {
                    ret = -2;
                    return ret;
                }
                ret =  tr.EnterRoom(webPlayer);
                rpv.RoomId = tr.RoomId;
            }
            return ret;
        }

        


        public int ExitRoom(WebPlayer webPlayer)
        {
            var ret = -1;
            var rpv = webPlayer.GetIBehaviour<RoomPlayerBv>();
            if (rpv == null)
            {
                return ret;
            }
            if (rpv.RoomId != string.Empty)
            {
               var tr = this.GetTableRoom(rpv.RoomId);
               if(tr==null)
                {
                    return ret;
                }
                ret = tr.ExitRoom(webPlayer);
                rpv.RoomId = string.Empty;
                if ( 0 == tr.Dict_Rp.Count)//
                {
                    tr.CloseRoom();
                }
            }

            return ret;
        }
   
        public override bool ShutDown()
        {
            foreach(var vk in m_Dict_Room)
            {
                vk.Value.ShutDown();
            }
            return base.ShutDown();
        }
        public override void Update()
        {
            foreach (var vk in m_Dict_Room)
            {
                vk.Value.Update(ClientTimer.Instance.DeltaTime, ClientTimer.Instance.UnScaledDeltaTime);
            }
        }
    }
}

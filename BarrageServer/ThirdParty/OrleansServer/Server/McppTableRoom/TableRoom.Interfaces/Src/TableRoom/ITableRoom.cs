using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class TableRoomInfo
    {
        public string Id;
        public List<TableUser_Data> Ls_User = new List<TableUser_Data>();
        public ETableRoomState TableRoomState;
        public int IMaxRoomPlayer;//最大人数



    }
    public enum ETableRoomState
    {
        ERoom_InActive,//没有激活的状态
        ERoom_Idle,//空闲没人状态
        ERoom_Ready,//有人正在准备组队
        ERoom_Loading,//所有人准备就绪准备开始游戏
        ERoom_InGame,//进入游戏
        ERoom_GameOver,//游戏结束返回Ready状态
    }

    public interface ITableRoom :IGrainWithStringKey
    {
        Task<bool> Init();
        Task<bool> ShutDown();

        Task<int> Join(TableUser_Data user);
        Task<int> Exit(string id);

        Task Update(float t);


        Task SendToAllPlayer(byte[] msg);//默认发给所有房间玩家
    }
}

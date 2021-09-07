using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class TableUser_Data
    {
        public string Id;
        public string NickName;
        public string AvatarUrl;
        public string RoomId;//房间号
        public bool IsReady;
        public int IPercent;
        public int Camp;//阵营 
        public int CampId;//小队编号
    }

    public interface ITableUser : IGrainWithStringKey
    {
        Task<TableUser_Data> GetTableUser_Data();
        Task SetTableUser_Data(TableUser_Data tu_data);
    }
}

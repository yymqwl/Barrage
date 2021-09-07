using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
namespace Mcpp
{
    public partial class GameConstant :TInstance<GameConstant>
    {
        public const byte TThreadInternal = 1;//10 ms刷新间隔
        /// 自动踢出玩家时间
        public const uint TPlayerIdle = 60;

        public const string Str_Stars = "Stars";
        ///////////////////////////////////DB
        public const string Str_Id = "Id";
        public const string Str_User_Data_Tb = "User_Data_Tb";//表

        public const string Str_User_Star_Tb = "User_Star_Tb";//表
        ////////////////
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    
    public class MatchingItem
    {
        public EMatchStrategyType MatchStrategyType;
        public WebPlayer Player;
        public int Score;
        public string TeamId;//队伍Id
        public EMatchingState MatchingState;
        public long St_Time;
    }
    public class MatchStrategy
    {
        public EMatchStrategyType MatchStrategyType;
        public int MaxPlayer;
    }
    
    public enum EMatchStrategyType
    {
        MatchStrategy_1V1 = 0,
        MatchStrategy_2V2,
        MatchStrategy_3V3,
    }
    public enum EMatchingType
    {
        Matching_Rank = 1,//排位
        Matching_Friend = 2,//好友匹配
    }
    public enum EMatchingState
    {
        Matching_Idle = 1,//
        Matching_Finish,//匹配完成
        //Matching_Confirm,//匹配确认
    }
}

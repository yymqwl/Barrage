using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RoomServer
{
    public class MatchingRank : MachingBase
    {
        protected List<MatchStrategy> m_Ls_MatchStrategy = new List<MatchStrategy>();

        protected List<MatchingItem> m_Ls_MatchingItems = new List<MatchingItem>();
        protected List<MatchingItem> m_Ls_NeedDel = new List<MatchingItem>();
        public override bool Init()
        {
            var pret = base.Init();
            m_Ls_MatchStrategy.Clear();

            {
                var ms1 = new MatchStrategy(){MaxPlayer=2, MatchStrategyType = EMatchStrategyType.MatchStrategy_1V1};
                m_Ls_MatchStrategy.Add(ms1);

                var ms2 = new MatchStrategy() { MaxPlayer = 4, MatchStrategyType = EMatchStrategyType.MatchStrategy_2V2 };
                m_Ls_MatchStrategy.Add(ms2);
            }

          

            return pret;
        }

        /*
        public List<MatchingItem> Get_SameGameType(int gt)
        {
            List<MatchingItem> tmp_ls = new List<MatchingItem>();
            m_Ls_MatchingItems.ForEach ((MatchingItem mib) =>
            {
            });
            return tmp_ls;
        }*/

        public override void Update()
        {
            m_Ls_NeedDel.Clear();
            for (var i = 0; i < m_Ls_MatchingItems.Count;)
            {
                var mi = m_Ls_MatchingItems[i];
                var ms = m_Ls_MatchStrategy[(int)mi.MatchStrategyType];

                if(i+ ms.MaxPlayer > m_Ls_MatchingItems.Count )
                {
                    break;
                }
                var tmp_ls = m_Ls_MatchingItems.GetRange(i, ms.MaxPlayer-1);
                if(CanMath(tmp_ls))
                {
                    i += tmp_ls.Count;
                    Match(tmp_ls);
                }
                else
                {
                    i++;
                }
            }

            for (var i = 0; i < m_Ls_MatchingItems.Count;)
            {
                var mi = m_Ls_MatchingItems[i];
                var ms = m_Ls_MatchStrategy[(int)mi.MatchStrategyType];
                var ts = TimeHelper.ClientNow() - mi.St_Time;//TimeHelper.DateTimeToUnixTimestamp_Now()-mi.St_Time;

                if(ts>=TMatching_AI_Time)
                {
                    if(mi.MatchStrategyType == EMatchStrategyType.MatchStrategy_1V1)
                    {
                        MatchAI(m_Ls_MatchingItems.GetRange(i,1));
                    }
                    else if(mi.MatchStrategyType == EMatchStrategyType.MatchStrategy_2V2)
                    {
                        var tmp_ls = m_Ls_MatchingItems.GetRange(i, m_Ls_MatchingItems.Count-1);
                        MatchAI(tmp_ls);
                    }
                }
            }


            foreach (var itemdel in m_Ls_NeedDel)
            {
                this.m_Ls_MatchingItems.Remove(itemdel);
            }
        }
        public bool CanMath(params MatchingItem[] matchingItems )
        {
            for(int i=0;i<matchingItems.Length;++i)
            {
                if(matchingItems[i].MatchStrategyType != matchingItems[i+1].MatchStrategyType)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CanMath(List<MatchingItem> ls_matchingItems)
        {
            for (int i = 0; i < ls_matchingItems.Count; ++i)
            {
                if (ls_matchingItems[i].MatchStrategyType != ls_matchingItems[i + 1].MatchStrategyType)
                {
                    return false;
                }
            }
            return true;
        }
        public void Match(List<MatchingItem> ls_matchingItems)
        {
            Log.Debug($"匹配:{ls_matchingItems.Count}");
            foreach (var mi in ls_matchingItems)
            {
                mi.MatchingState = EMatchingState.Matching_Finish;
                m_Ls_NeedDel.Add(mi);
            }
        }
        /*
        public void Match(params MatchingItem[] matchingItems)
        {
            Log.Debug($"匹配:{matchingItems.Length}");
            foreach(var mi in matchingItems)
            {
                mi.MatchingState = EMatchingState.Matching_Finish;
                m_Ls_NeedDel.Add(mi);
            }
        }*/
        public void MatchAI( List<MatchingItem> ls_matchingItems)
        {
            Log.Debug($"匹配AI:{ls_matchingItems.Count}");
            foreach (var mi in ls_matchingItems)
            {
                mi.MatchingState = EMatchingState.Matching_Finish;
                m_Ls_NeedDel.Add(mi);
            }
        }
        public override bool ShutDown()
        {

            return base.ShutDown();
        }

    }
}

using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoomServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GameMainEntry.Instance.Entry(args);
        }
    }
}

/*
           List<int> ls = new List<int>();
            ls.Add(1);
            ls.Add(2);
            ls.Add(3);
            var ls2= ls.GetRange(1,2);
            {
                List<MatchingItem> Ls_MI = new List<MatchingItem>();
Ls_MI.Add(new MatchingItem() { TeamId = "1", Score = 0, MatchingType = EMatchingType.Matching_Friend});
                Ls_MI.Add(new MatchingItem() { TeamId = "", Score = 20, MatchingType = EMatchingType.Matching_Friend });
                Ls_MI.Add(new MatchingItem() { TeamId = "1", Score = 20, MatchingType = EMatchingType.Matching_Friend });
                Ls_MI.Add(new MatchingItem() { TeamId = "", Score = 20, MatchingType = EMatchingType.Matching_Friend });

                Ls_MI.Add(new MatchingItem() { TeamId = "2", Score = 10, MatchingType = EMatchingType.Matching_Rank });
                Ls_MI.Add(new MatchingItem() { TeamId = "4", Score = 20, MatchingType = EMatchingType.Matching_Friend });


                Ls_MI.Sort((x, y) =>
                {
                    var imt = x.MatchingType - y.MatchingType;
                    if (imt !=0)
                    {
                        return imt;
                    }
                    var iteamdist = y.TeamId.CompareTo(x.TeamId);
                    if(iteamdist !=0)
                    {
                        return iteamdist;
                    }
                    return x.Score - y.Score;
                });
                Ls_MI.ForEach((mi) => Log.Debug(JsonHelper.JsonSerialize_String(mi)));
            }
*/

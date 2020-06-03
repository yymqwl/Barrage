using GameFramework;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Mcpp
{
    public class Leaderboard_User
    {
        public string Id;
        public string NickName;
        public string AvatarUrl;
        public int Score;
    }
    [GameFrameworkModule]
    public class LeaderboardModule : GameFrameworkModule
    {
        public override int Priority => 200;
        protected DateTimeTrigger m_NextRank_Settlement = new DateTimeTrigger();
        protected DateTimeTrigger m_NextRank_Refresh = new DateTimeTrigger();

        protected List<Leaderboard_User> m_Leaderboard_Stars = new List<Leaderboard_User>();

        public List<Leaderboard_User> Leaderboard_Stars
        {
            get { return m_Leaderboard_Stars; }
        }
        public override bool Init()
        {
            var pret = base.Init();

            /////////////下次结算
            //this.MakeRankSettlement(null, null);
            var tnextweek = this.GetNextRankWeekDist();
            m_NextRank_Settlement.Start(tnextweek, this.MakeRankSettlement);
            //////////

            ///////刷新当前
            this.NextRank_Refresh(null, null);
            //Log.Debug(tnexthour.ToString() );
            //Log.Debug("LeaderboardModule: Init" + this.Priority);
            //this.GetRankStar("oMRh0wp4QddVH4RAz-gpw8uUiekM");
            return pret;
        }

        
        public void RecordStars(WebPlayer wp)
        {
            var db = GameMainEntry.Instance.DbModule.Db;

            var score = wp.User_Data.Value<int>(GameConstant.Str_Stars);
            db.SortedSetAddAsync(GameConstant.Str_User_Star_Tb,wp.Id, score);
        }


        public void MakeRankSettlement(object param1,object param2)
        {
           Log.Debug("MakeRankSettlement 结算");
           var db = GameMainEntry.Instance.DbModule.Db;

           db.KeyDeleteAsync(GameConstant.Str_User_Star_Tb);

           var tnextweek = this.GetNextRankWeekDist();
           m_NextRank_Settlement.Start(tnextweek, this.MakeRankSettlement);
        }
        public void NextRank_Refresh(object param1, object param2)
        {
            Log.Debug("NextRank_Refresh");
            var db = GameMainEntry.Instance.DbModule.Db;
            var ls_key = db.SortedSetRangeByScore(GameConstant.Str_User_Star_Tb, 0, int.MaxValue , StackExchange.Redis.Exclude.None, StackExchange.Redis.Order.Descending,0,100);
            //去前100名;
            m_Leaderboard_Stars.Clear();
            for(var i=0;i<ls_key.Length;++i)
            {
                var rd_data = db.HashGet(GameConstant.Str_User_Data_Tb, ls_key[i]);
                var lb_user = new Leaderboard_User();
                if(rd_data.IsNullOrEmpty)
                {
                    continue;
                }
                var jobj = JObject.Parse(rd_data);
                lb_user.Id = jobj["Id"].ToString();
                lb_user.NickName = jobj["NickName"].ToString();
                lb_user.AvatarUrl = jobj["AvatarUrl"].ToString();
                lb_user.Score = (int)jobj[GameConstant.Str_Stars];
                this.m_Leaderboard_Stars.Add(lb_user);
            }


            var tnexthour = this.GetNextRankHour();
            m_NextRank_Refresh.Start(tnexthour, this.NextRank_Refresh);
        }
        public long GetRankStar_Index(string id)
        {
            var db = GameMainEntry.Instance.DbModule.Db;
            var ipaiming = db.SortedSetRank(GameConstant.Str_User_Star_Tb, id, StackExchange.Redis.Order.Descending);
            if(ipaiming == null)
            {
                ipaiming = -1;
            }
            else
            {
                ipaiming++;
            }
            return ipaiming.Value;
        }
        public  DateTime GetNextRankWeekDist()
        {
            DateTime dtnow = DateTime.Now;
            var dtstart = new DateTime(dtnow.Year, 1, 1);
            var iweek =  (int)((dtnow - dtstart).TotalDays/7);//一周7天
            var inextweek = iweek + 1;

            var tnext = dtstart + new TimeSpan(inextweek*7,0,0,0);
            return tnext;
        }
        public DateTime GetNextRankHour()
        {
            DateTime dtnow = DateTime.Now;
            dtnow=dtnow.AddMinutes(1);
            //dtnow.AddHours(1);
            DateTime dtnext = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day, dtnow.Hour,0,0);
            return dtnext;
        }
        /*
        public DateTime GetNextHourTime()
        {

        }*/
        public override bool ShutDown()
        {
            var pret = base.ShutDown();


            //Log.Debug("LeaderboardModule ShutDown:" + this.Priority);
            return pret;
        }
        public override void Update()
        {


        }
    }
}

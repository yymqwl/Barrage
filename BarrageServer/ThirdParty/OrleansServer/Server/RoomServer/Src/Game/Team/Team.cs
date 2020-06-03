using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    
    public class Team : ATeam
    {
        public List<TeamPlayer_Data> GetTeamPlayer_Data()
        {
            var Ls_TeamPlayer_Data = new List<TeamPlayer_Data>();
            foreach (var vk in this.Dict_Rp)
            {
                var tpbv = vk.Value.GetIBehaviour<TeamPlayerBv>();
                if (tpbv != null && tpbv.TeamPlayer_Data != null)
                {
                    Ls_TeamPlayer_Data.Add(tpbv.TeamPlayer_Data);
                }
            }
            return Ls_TeamPlayer_Data;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            this.CheckInActivePy();
        }

    }



    /*
    public string Id;
    public List<TeamMember> Ls_TeamMember = new List<TeamMember>();
        
    public class TeamMember
    {
        public int Camp;
        public int CampId;//0是队长
        public WebPlayer WebPlayer;//
    }
    */
}

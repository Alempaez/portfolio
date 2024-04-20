using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot2.Models
{
    public class Profile
    {
        public int account_id { get; set; }
        public string personaname { get; set; }
        public object name { get; set; }
        public bool plus { get; set; }
        public int cheese { get; set; }
        public string steamid { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public string profileurl { get; set; }
        public DateTime? last_login { get; set; }
        public object loccountrycode { get; set; }
        public object status { get; set; }
        public bool fh_unavailable { get; set; }
        public bool is_contributor { get; set; }
        public bool is_subscriber { get; set; }
    }

    public class RootProfile
    {
        public Profile profile { get; set; }
        public int rank_tier { get; set; }
        public object leaderboard_rank { get; set; }
        public string exMessage { get; set; }
    }
}

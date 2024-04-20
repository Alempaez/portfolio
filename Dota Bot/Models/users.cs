using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot2.Data
{
    public class users
    {
        public string discord_id { get; set; }
        public string steam_id { get; set; }
        public int dota_id { get; set; }
        public users(string discord_id, string steam_id, int dota_id)
        {
            this.discord_id = discord_id;
            this.steam_id = steam_id;
            this.dota_id = dota_id;
        }
    }
}

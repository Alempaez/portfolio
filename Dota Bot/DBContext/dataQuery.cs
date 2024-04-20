using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TutorialBot2.Models;

namespace TutorialBot2.Data
{
    public class dataQuery
    {
        public async Task<lastmatches[]> matchQuery(int playerID)
        {
            string url = $"https://api.opendota.com/api/players/{playerID}/recentMatches";
            string respuesta = GetHttp(url);
            var lastMatches = JsonConvert.DeserializeObject <lastmatches[]>(respuesta);
            return lastMatches;

        }

        public async Task<RootProfile> playerQuery(int playerID)
        {
            RootProfile playerData = new RootProfile();
            try
            {
                string url = $"https://api.opendota.com/api/players/{playerID}";
                string respuesta = GetHttp(url);
                playerData = JsonConvert.DeserializeObject<RootProfile>(respuesta);
                return playerData;
            }
            catch(Exception ex)
            {
                playerData.exMessage = ex.Message;
                return null;
            }

        }
        public async Task<heroes[]> heroQuery()
        {
            string url = $"https://api.opendota.com/api/heroes";
            string respuesta = GetHttp(url);
            var heroData = JsonConvert.DeserializeObject<heroes[]>(respuesta);
            return heroData;

        }

        public static string GetHttp(string url)
        {
            WebRequest oRequest = WebRequest.Create(url);
            oRequest.Method = "GET";
            oRequest.ContentType = "application/json; charset = UTF-8";
            WebResponse oResponse =  oRequest.GetResponse();
            StreamReader sr = new StreamReader(oResponse.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }


    }
}

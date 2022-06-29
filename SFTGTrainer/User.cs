using System;
using Steamworks;

namespace SFTGTrainer
{
    [Serializable]
    public class User
    {
        public string username = "";
        public string SteamID64 = "";
        public string rank = "";
        public bool banned = false;

        public User(string username, string SteamID64, string rank, bool banned)
        {
            this.username = username;
            this.SteamID64 = SteamID64;
            this.rank = rank;
            this.banned = banned;
        }

        public User()
        {
            this.username = "";
            this.SteamID64 = "";
            this.rank = "";
            this.banned = false;
        }

        public CSteamID GetSteamID()
        {
            return new CSteamID(Convert.ToUInt64(this.SteamID64));
        }

        public bool isVIP()
        {
            return this.rank == "vip"; 
        }

        public bool isDev()
        {
            return this.rank == "dev";
        }
    }
}

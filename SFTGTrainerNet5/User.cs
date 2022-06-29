using System;
using Steamworks;

namespace SFTGTrainer
{
    public class User
    {
        public string username;
        public CSteamID SteamID64;
        public string rank;
        public bool banned;

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

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace SFTGTrainer
{
    public class UserManager : MonoBehaviour
    {
        public static readonly string usersURL = "https://resources.redboxing.fr/stickFight/users.json";

        public Dictionary<CSteamID, User> UsersList = new Dictionary<CSteamID, User>();
        public User LocalUser;
        public bool loaded = false;

        public IEnumerator Start()
        {
            LogUtils.debug("Fetching users list");
            WWW www = new WWW(usersURL);
            yield return www;

            if (www.error == null)
            {
                processJSON(www.text);
            }
            else
            {
                LogUtils.error("ERROR: " + www.error);
            }

            loaded = true;
            www.Dispose();
            yield break;
        }

        private IEnumerator fetchJSON()
        {
            LogUtils.debug("fetchJSON");

            WWW www = new WWW(usersURL);
            yield return www;

            if (www.error == null)
            {
                processJSON(www.text);
            }
            else
            {
                LogUtils.error("ERROR: " + www.error);
            }

            loaded = true;
            www.Dispose();
            yield break;
        }

        private void processJSON(string json)
        {
            LogUtils.debug("Loading users list");

            List<User> users = JsonMapper.ToObject<List<User>>(json);
            foreach (User usr in users)
            {

                if (!UsersList.ContainsKey(usr.GetSteamID()))
                    UsersList.Add(usr.GetSteamID(), usr);

                if (usr.GetSteamID() == SteamUser.GetSteamID()) LocalUser = usr;
            }

            LogUtils.info("Users loaded !");
        }
    }

    [Serializable]
    public class RootObject
    {
        public List<User> users = new List<User>();
    }
}

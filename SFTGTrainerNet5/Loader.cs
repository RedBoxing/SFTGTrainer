using SFTGTrainer.ModMenu;
using SFTGTrainer.ModMenu.Elements;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SFTGTrainer
{
    public static class Loader
    {
        public static Dictionary<CSteamID, User> UsersList = new Dictionary<CSteamID, User>();
        public static User LocalUser;

        public static GameObject gameObject;
        public static bool initialized;

        public static void Load()
        {
            if (!initialized)
            {
                while (!SteamManager.Initialized)
                {
                    Thread.Sleep(200);
                    Console.WriteLine("Waiting...");
                }

                if (loadUsers().Result)
                {
                    if (UsersList != null && UsersList.ContainsKey(SteamUser.GetSteamID())) LocalUser = UsersList[SteamUser.GetSteamID()];
                    if (LocalUser != null && LocalUser.banned || LocalUser == null) Application.Quit();

                    if (LocalUser.isDev())
                        AllocConsoleHandler.Open();

                    LogUtils.info("Loading Trainer..");

                    gameObject = new GameObject();
                    gameObject.AddComponent<Trainer>();
                    UnityEngine.Object.DontDestroyOnLoad(gameObject);

                    initialized = true;
                }
            }
        }

        public static void Unload()
        {
            if (!initialized) return;
            List<GameObject> objs = new List<GameObject>();
            Menu menu = Menu.Instance;
            foreach (MenuElement obj in menu.GetElements())
            {
                foreach (Transform child in obj.transform)
                {
                    objs.Add(child.gameObject);
                }
                objs.Add(obj.gameObject);
            }

            foreach (GameObject obj in objs)
            {
                UnityEngine.Object.Destroy(obj);
            }
        }

        public static async Task<bool> loadUsers()
        {
            string url = "https://resources.redboxing.fr/stickFight/users.json";
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.Send();

            if (www.error == null)
            {
                LogUtils.debug("Loading users list");

                foreach (User usr in JsonUtility.FromJson<List<User>>(www.downloadHandler.text))
                {
                    LogUtils.debug(usr.username);

                    if (!UsersList.ContainsKey(usr.SteamID64))
                        UsersList.Add(usr.SteamID64, usr);
                }

                LogUtils.info("Users loaded !");
                return true;
            }
            else
            {
                LogUtils.error("ERROR: " + www.error);
                return false;
            }
        }
    }
}

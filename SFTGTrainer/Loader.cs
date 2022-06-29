using SFTGTrainer.ModMenu;
using SFTGTrainer.ModMenu.Elements;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace SFTGTrainer
{
    public class Loader
    {
        public static GameObject gameObject;
        public static GameObject userManagerObj;

        public static bool initialized;

        public static void Load()
        {
            LogUtils.info("SFTGTrainer Injected !");

            if (!initialized)
            {
                LogUtils.info("Initializing...");

                while (!SteamManager.Initialized)
                {
                    Thread.Sleep(200);
                    LogUtils.info("Waiting for Steam to initialize...");
                }

                userManagerObj = new GameObject();
                UserManager userManager = userManagerObj.AddComponent<UserManager>();

                while (!userManager.loaded) Thread.Sleep(200);

                LogUtils.info("Checking User..");
               
                if (userManager.LocalUser == null || userManager.LocalUser != null && userManager.LocalUser.banned) return;
                if (userManager.LocalUser.isDev()) AllocConsoleHandler.Open();

                LogUtils.info("Loading Trainer..");

                gameObject = new GameObject();
                gameObject.AddComponent<Trainer>();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                UnityEngine.Object.DontDestroyOnLoad(userManager);

                initialized = true;
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

            objs.Add(menu.gameObject);
            objs.Add(gameObject);
            objs.Add(userManagerObj);

            foreach (GameObject obj in objs)
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }
}

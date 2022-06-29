using System.Collections;
using UnityEngine;
using SFTGTrainer.ModMenu;
using SFTGTrainer.ModMenu.Elements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace SFTGTrainer
{
    class Trainer : MonoBehaviour
    {
        private Menu menu;

        public ControllerHandler controllerHandler = null;
        public GameManager gameManager = null;

        public bool isFirstStart = true;
        public bool FullAuto = false;
        public bool UncappedFirerate = false;
        public bool UnlimitedHealth = false;
        public bool UnlimitedAmmo = false;
        public bool NoRecoil = false;
        public bool FlightMode = false;

        public IEnumerator Init()
        {
            yield return new WaitForSeconds(5);

            if (menu == null) menu = Menu.Instance;
            menu.StartRendering(-600, 750, 10, 40, 280, 5, 18);

            menu.RegisterElement(menu.CreateLabel("STICKFIGHT TRAINER", 25));

            MenuToggleableButton unlimitedHealthBtn = menu.CreateToogleableButton("Unlimited Health");
            unlimitedHealthBtn.Toggled = toggleUnlimitedHealth;

            MenuToggleableButton unlimitedAmmoBtn = menu.CreateToogleableButton("Unlimited Ammo");
            unlimitedAmmoBtn.Toggled = toggleUnlimitedAmmo;

            MenuToggleableButton uncappedFirerateBtn = menu.CreateToogleableButton("Uncapped Firerate");
            uncappedFirerateBtn.Toggled = toggleUncappedFirerate;

            MenuToggleableButton noRecoilBtn = menu.CreateToogleableButton("No Recoil");
            noRecoilBtn.Toggled = toggleNoRecoil;

            MenuToggleableButton fullAutoBtn = menu.CreateToogleableButton("Full Auto");
            fullAutoBtn.Toggled = toggleFullAuto;

            MenuToggleableButton flightModeBtn = menu.CreateToogleableButton("Flight Mode");
            flightModeBtn.Toggled = toggleFlightMode;

            MenuSlider switchWeaponBtn = menu.CreateSlider("Weapon");
            switchWeaponBtn.Change = switchWeapon;

            MenuButton spawnWeaponBtn = menu.CreateButton("Spawn Random Weapon");
            spawnWeaponBtn.Pressed = spawnRandomWeapon;

            menu.RegisterElement(unlimitedHealthBtn);
            menu.RegisterElement(unlimitedAmmoBtn); 
            menu.RegisterElement(uncappedFirerateBtn);
            menu.RegisterElement(noRecoilBtn);
            menu.RegisterElement(fullAutoBtn);
            menu.RegisterElement(flightModeBtn);
            menu.RegisterElement(switchWeaponBtn);
            menu.RegisterElement(spawnWeaponBtn);

            menu.RegisterElement(menu.CreateLabel("Trainer by RedBoxing\nhttps://redboxing.fr"));

            menu.ChangeMode(true);
            yield return null;
        }

        public void Start()
        {
            menu = Menu.CreateModMenuObject();
            StartCoroutine(nameof(Init));

            this.controllerHandler = GameObject.Find("GameManagement").GetComponent<ControllerHandler>();
            this.gameManager = GameObject.Find("GameManagement").GetComponent<GameManager>();
           

            if (isFirstStart)
            {
                SceneManager.sceneLoaded += this.OnSceneLoaded;
                this.gameManager.winText.fontSize = 140f;
                this.gameManager.winText.color = Color.red;
                this.gameManager.winText.text = "StickFight Trainer by RedBoxing";
                this.gameManager.winText.gameObject.SetActive(true);
            }
        }

        public void Update()
        {
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        Fighting fighting = Utils.GetFieldValue<Fighting>(controller, "fighting");
                        NetworkPlayer networkPlayer = Utils.GetFieldValue<NetworkPlayer>(fighting, "mNetworkPlayer");
                        HealthHandler healthHandler = Utils.GetFieldValue<HealthHandler>(networkPlayer, "mHealthHandler");

                        if (this.UnlimitedHealth)
                        {
                            healthHandler.maxHealth = float.MaxValue;
                            healthHandler.health = float.MaxValue;
                        }

                        if (this.UnlimitedAmmo && fighting.weapon != null)
                        {
                            fighting.weapon.currentCharge = 9999f;
                            fighting.weapon.secondsOfUseLeft = 9999f;
                            Utils.SetFieldValue<int>(fighting, "bulletsLeft", 9999);
                        }

                        if (this.UncappedFirerate && fighting.weapon != null)
                        {
                            fighting.weapon.cd = 0f;
                            Utils.SetFieldValue<bool>(fighting.weapon, "reloads", false);
                            fighting.weapon.reloadTime = 0f;
                            fighting.weapon.shotsBeforeReload = 9999;
                        }

                        if (this.NoRecoil && fighting.weapon != null)
                        {
                            fighting.weapon.recoil = 0f;
                            fighting.weapon.torsoRecoil = 0f;
                        }

                        if (this.FullAuto && fighting.weapon != null)
                        {
                            fighting.weapon.fullAuto = true;
                            fighting.fullAuto = true;
                        }

                        if (this.FlightMode)
                        {
                            controller.canFly = true;
                        }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                ((MenuToggleableButton)menu.elements[6]).Toggle();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ((MenuButton)menu.elements[8]).Pressed();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                ((MenuSlider)menu.elements[7]).Decrease();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                ((MenuSlider)menu.elements[7]).Increase();
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                switchWeapon(((MenuSlider)menu.elements[7]).value);
            }

            if (Input.GetKeyUp(KeyCode.End))
            {
                Loader.Unload();
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.controllerHandler = GameObject.Find("GameManagement").GetComponent<ControllerHandler>();
            this.gameManager = GameObject.Find("GameManagement").GetComponent<GameManager>();
        }

        void toggleUnlimitedHealth(bool state)
        {
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        if (!state)
                        {
                            Fighting obj = Utils.GetFieldValue<Fighting>(controller, "fighting");
                            NetworkPlayer obj2 = Utils.GetFieldValue<NetworkPlayer>(obj, "mNetworkPlayer");
                            HealthHandler healthHandler = Utils.GetFieldValue<HealthHandler>(obj2, "mHealthHandler");
                            healthHandler.maxHealth = 100f;
                            healthHandler.health = 100f;
                        }
                    }
                }
            }
            this.UnlimitedHealth = state;
        }

        void toggleUnlimitedAmmo(bool state)
        {
            this.UnlimitedAmmo = state;
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        if (!state)
                        {
                            Fighting fighting = Utils.GetFieldValue<Fighting>(controller, "fighting");
                            if (fighting.weapon != null)
                            {
                                fighting.weapon.currentCharge = 30f;
                                fighting.weapon.secondsOfUseLeft = 30f;
                                Utils.SetFieldValue<int>(fighting, "bulletsLeft", 30);
                            }
                        }
                    }
                }
            }
        }

        void toggleUncappedFirerate(bool state)
        {
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        if (!state)
                        {
                            Fighting fighting = Utils.GetFieldValue<Fighting>(controller, "fighting");
                            if (fighting.weapon != null)
                            {
                                fighting.weapon.cd = 0.1f;
                                Utils.SetFieldValue<bool>(fighting.weapon, "reloads", true);
                                fighting.weapon.reloadTime = 1f;
                                fighting.weapon.shotsBeforeReload = 1;
                            }
                        }
                    }
                }
            }
            this.UncappedFirerate = state;
        }

        void toggleNoRecoil(bool state)
        {
            this.NoRecoil = state;
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        if (!state)
                        {
                            Fighting fighting = Utils.GetFieldValue<Fighting>(controller, "fighting");
                            if (fighting.weapon != null)
                            {
                                fighting.weapon.recoil = 1f;
                                fighting.weapon.torsoRecoil = 1f;
                            }
                        }
                    }
                }
            }
        }

        void toggleFullAuto(bool state)
        {
            this.FullAuto = state;
        }

        void toggleFlightMode(bool state)
        {
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        if (!state)
                        {
                            controller.canFly = false;
                        }
                    }
                }
            }
            this.FlightMode = state;
        }

        void switchWeapon(float index)
        {
            if (this.controllerHandler != null && this.controllerHandler.ActivePlayers != null)
            {
                foreach (Controller controller in this.controllerHandler.ActivePlayers)
                {
                    if (controller.HasControl)
                    {
                        Fighting fighting = Utils.GetFieldValue<Fighting>(controller, "fighting");
                        NetworkPlayer obj = Utils.GetFieldValue<NetworkPlayer>(fighting, "mNetworkPlayer");
                        ChatManager chatManager = Utils.GetFieldValue<ChatManager>(obj, "mChatManager");
                        fighting.Dissarm();
                        fighting.NetworkPickUpWeapon((byte)index);
                        bool isNetworkMatch = MatchmakingHandler.IsNetworkMatch;
                        if (isNetworkMatch)
                        {
                            chatManager.Talk(fighting.weapon.name);
                        }
                    }
                }
            }
        }

        void spawnRandomWeapon()
        {
            Vector3 vector = Vector3.up * 11f + Vector3.forward * Random.Range(0f, 8f);
            GameObject original = null;
            WeaponSelectionHandler weaponSelectionHandler = Utils.GetFieldValue<WeaponSelectionHandler>(this.gameManager, "m_WeaponSelectionHandler");
            int randomWeaponIndex = weaponSelectionHandler.GetRandomWeaponIndex(true, out original);
            bool flag = randomWeaponIndex < 0;
            if (!flag)
            {
                bool isNetworkMatch = MatchmakingHandler.IsNetworkMatch;
                if (isNetworkMatch)
                {
                    MultiplayerManager multiplayerManager = Utils.GetFieldValue<MultiplayerManager>(this.gameManager, "mNetworkManager");
                    multiplayerManager.SpawnWeapon(randomWeaponIndex, vector, true);
                }
                else
                {
                    GameObject gameObject = Object.Instantiate<GameObject>(original, vector, Quaternion.identity);
                    List<Rigidbody> list = Utils.GetFieldValue<List<Rigidbody>>(this.gameManager, "mSpawnedWeapons");
                    list.Add(gameObject.GetComponent<Rigidbody>());
                }
            }
        }
    }
}

#pragma warning disable 0618
using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Core.Interaction;
using Assets.Scripts.Testing;
using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instance;
        public static bool GameLoaded;
        public static bool GamePaused = false;
        public static bool CutsceneActive;
        public UserSave UserSave;
        public static bool ShowUI = true;
        public ImageContainer LoadingScreen;
        void Awake()
        {
            Instance = this;
            LoadGame();
            GameLoaded = true;
            CutsceneActive = false;
            LoadingScreen = Rm_RPGHandler.Instance.Customise.LoadingScreen;
        }

        void Update()
        {
            if (CutsceneActive) return;

            Time.timeScale = GamePaused ? 0 : 1;
        }

        private void LoadGame()
        {
            GameDataSaveLoadManager.Instance.LoadIfNotLoaded();
            UserSaveLoadManager.Instance.LoadUserData();
            GameSettingsSaveLoadManager.Instance.LoadSettings();


            var npcGameObjects = GameObject.FindGameObjectsWithTag("NPC");
            var npcMonoObjects = npcGameObjects.Select(n => n.GetComponent<NpcCharacterMono>()).ToList();
            foreach (var npc in npcMonoObjects)
            {
                if (Rm_RPGHandler.Instance.Repositories.Interactable.AllNpcs.FirstOrDefault(i => i.ID == npc.NpcID) != null)
                {
                    var npcData = Rm_RPGHandler.Instance.Repositories.Interactable.AllNpcs.First(i => i.ID == npc.NpcID);
                    npc.SetNPC(npcData);
                }
                else
                {
                    Debug.LogError("Could not find NPC data for NPC: " + npc.NpcID + " Destroying.");
                    Destroy(npc.gameObject);
                }
            }

            var interactObjects = GameObject.FindGameObjectsWithTag("Interactable");
            var interactMonos = interactObjects.Select(n => n.GetComponent<InteractiveObjectMono>()).ToList();
            foreach (var interactable in interactMonos)
            {
                var interactableObj = Rm_RPGHandler.Instance.Repositories.Interactable.AllInteractables.FirstOrDefault(i => i.ID == interactable.ObjectID);
                if (interactableObj != null)
                {
                    interactable.SetInteractable(interactableObj);
                }
                else
                {
                    Debug.LogError("Could not find Interactable data for Interactable: " + interactable.InteractiveObject.ID);
                    Destroy(interactable.gameObject);
                }
            }

            var harvestObjects = GameObject.FindGameObjectsWithTag("Harvestable");
            var harvestMonos = harvestObjects.Select(n => n.GetComponent<InteractableHarvestable>()).ToList();
            foreach (var harvestable in harvestMonos)
            {
                var harvestableObj = Rm_RPGHandler.Instance.Harvesting.HarvestableDefinitions.FirstOrDefault(i => i.ID == harvestable.ObjectID);
                if (harvestableObj != null)
                {
                    harvestable.SetHarvestable(harvestableObj);
                }
                else
                {
                    Debug.LogError("Could not find harvestable data for Harvestable: " + harvestable.ObjectID);
                    Destroy(harvestable.gameObject);
                }
            }

            var enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
            var enemyMonos = enemyObjects.Select(n => n.GetComponent<EnemyCharacterMono>()).ToList();
            foreach (var enemy in enemyMonos)
            {
                if (enemy == null)
                {
                    Debug.Log("GameObject with Enemy tag has no EnemyCharacterMono");
                    continue;
                }

                var enemyChar = Rm_RPGHandler.Instance.Repositories.Enemies.AllEnemies.FirstOrDefault(i => i.ID == enemy.EnemyID);
                if (enemyChar != null)
                {
                    enemy.SetEnemy(enemyChar);
                }
                else
                {
                    Debug.LogError("Could not find Enemy data for: " + enemy.EnemyID);
                    Destroy(enemy.gameObject);
                }
            }



            var loaded = PlayerSaveLoadManager.Instance.LoadLastSaved();

            if(!loaded)
            {
                Debug.Log("Error loading last saved.");
                GameLoaded = false;
                Application.LoadLevel(0);
                return;
            }

            Debug.Log("Loaded user save and settings.");
            GetObject.RPGCamera.Init();
        }

        public void LoadLevel(string levelName, bool updatePlayerLevel, bool saveCurrentPlayer, WorldArea worldArea = null, Location location = null)
        {
            if(RPG.LoadLevel(levelName,updatePlayerLevel, saveCurrentPlayer,  worldArea, location))
            {
                GetObject.AudioPlayer.StartAudio();
                InitialiseLevel();
            }
        }

        private void InitialiseLevel()
        {
            LoadGame();
        }

        void Start()
        {
            GetObject.AudioPlayer.StartAudio(); 
            GetObject.UIHandler.Init(); 
        }

        //Todo: Move to OptionsMenuGUI and AutoSave.cs
        void OnGUI()
        {
            //todo: loading screen(s)
            if(Application.isLoadingLevel)
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), LoadingScreen.Image);
            }
        }

    }
}
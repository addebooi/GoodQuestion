using System;
using System.Linq;
using Assets.Scripts.Beta.NewImplementation;
using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LogicSpawn.RPGMaker.Core
{
    public class CharacterCreationMono : MonoBehaviour
    {
        public static CharacterCreationMono Instance;
        public GameObject ClassContainer;
        public GameObject ClassSelectPrefab;
        public GameObject SpawnPosition;
        public GameObject SpawnedClass;
        public Button CreateButton;
        public InputField CharacterNameTextField;
        public Text SelectedClassName;
        public PlayerSave PlayerToCreate;
        public string SaveName;
        public string ClassID;
        public string charName;

        void Awake()
        {
            Instance = this;
            SaveName = "Test_Save_" + Random.Range(0, 999);
            ClassID = "";
            charName = "Character";

            CreateClassButtons();
        }

        void Start()
        {
            SetClass(RPG.Player.Classes[0].ID);
        }

        private void CreateClassButtons()
        {
            ClassContainer.transform.DestroyChildren();
            foreach (var playerClass in RPG.Player.Classes)
            {
                var go = Instantiate(ClassSelectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(ClassContainer.transform, false);
                var classSelect = go.GetComponent<ClassSelectModel>();
                classSelect.ClassID = playerClass.ID;
                
                if(classSelect.ButtonText != null)
                    classSelect.ButtonText.text = playerClass.Name;

                if(classSelect.ButtonImage != null)
                    classSelect.ButtonImage.sprite = GeneralMethods.CreateSprite(playerClass.Image);
            }
        }

        public void ReturnToMainMenu()
        {
            Application.LoadLevel("MainMenu");
        }

        public void CreatePlayer()
        {
            var classDef = Rm_RPGHandler.Instance.Player.ClassDefinitions.First(c => c.ID == ClassID);

            var startingScene = classDef.StartingScene;
            var startingWorldArea = classDef.StartingWorldArea;
            var startingLocation = classDef.StartingLocation;

            PlayerToCreate = new PlayerSave(SaveName, ClassID, charName, startingScene);
            PlayerToCreate.Initialize();
            PlayerToCreate.WorldMap.CurrentWorldAreaID = startingWorldArea;
            PlayerToCreate.WorldMap.CurrentLocationID = startingLocation;

            if (classDef.StartAtWorldLocation)
            {
                var locationScene =
                    Rm_RPGHandler.Instance.Customise.WorldMapLocations.First(w => w.ID == startingWorldArea).Locations.
                        First(l => l.ID == startingLocation).SceneName;
                PlayerToCreate.CurrentScene = locationScene;
            }

            PlayerSaveLoadManager.Instance.Save(PlayerToCreate,true);
            RPG.LoadLevel(PlayerToCreate.CurrentScene,false,false);
        }
        
        void Update()
        {
            CreateButton.interactable = !string.IsNullOrEmpty(charName) && !string.IsNullOrEmpty(ClassID);
            charName = CharacterNameTextField.text;
        }

        public void SetClass(string classId)
        {
            Destroy(SpawnedClass);
            var classes = RPG.Player.Classes;
            var newClass = classes.First(c => c.ID == classId);

            SelectedClassName.text = newClass.Name;
            ClassID = classId;
            SpawnedClass = null;
            SpawnedClass = (GameObject)Instantiate(Resources.Load(newClass.ClassPrefabPath), SpawnPosition.transform.position - new Vector3(0,1,0), SpawnPosition.transform.rotation);
            var comp = SpawnedClass.GetComponents(typeof (MonoBehaviour));
            var model = SpawnedClass.GetComponent<RPGController>().CharacterModel;
            var idleAnimation = newClass.LegacyAnimations.CombatIdleAnim;
            foreach(var com in comp)
            {
                Destroy(com);
            }
            Destroy(SpawnedClass.GetComponent<NavMeshAgent>());
            Destroy(SpawnedClass.GetComponent<NavMeshObstacle>());
            Destroy(SpawnedClass.GetComponent<CapsuleCollider>());
            Destroy(SpawnedClass.GetComponent<CharacterController>());

            var modelAnim = model.GetComponent<Animation>();
            if(modelAnim != null)
            {
                modelAnim[idleAnimation.Animation].wrapMode = WrapMode.Loop;
                modelAnim.CrossFade(idleAnimation.Animation);
            }
        }
    }
}
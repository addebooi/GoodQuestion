using System;
using System.IO;
using System.Linq;
using LogicSpawn.RPGMaker.Editor.New;
using UnityEngine;
using UnityEditor;

namespace LogicSpawn.RPGMaker.Editor
{
    public class Rme_Tools_Toolbar : EditorWindow
    {

        private static Rme_Tools_Toolbar Window;

        private Texture2D PrefabBrowserIcon;
        private Texture2D SaveIcon;
        private Texture2D LoadIcon;
        private Texture2D NewSceneIcon;
        private Texture2D CombatIcon;
        private Texture2D DialogIcon;
        private Texture2D EventIcon;
        private Texture2D AchievementIcon;
        private Texture2D MapIcon;

        [MenuItem("Tools/LogicSpawn RPG All In One/Tools/Toolbar", false, 3)]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            Window = (Rme_Tools_Toolbar)GetWindow(typeof(Rme_Tools_Toolbar));
            Window.maxSize = new Vector2(2000, 2000);
            Window.titleContent = new GUIContent("RPGAIOToolbar");
            Window.minSize = new Vector2(80.1F, 80.1F);
            Window.position = new Rect(100, 100, 1000, 80);
        }

        [MenuItem("Tools/LogicSpawn RPG All In One/Tools/Create New Scene", false, 3)]
        private static void CreateNewScene()
        {
            AddScene();
        }
        [MenuItem("Tools/LogicSpawn RPG All In One/Tools/Add Scene Essentials", false, 4)]
        private static void AddSceneEssentialsMenu()
        {
            AddSceneEssentials();
        }
        [MenuItem("Tools/LogicSpawn RPG All In One/Tools/Clear Player Prefs", false, 4)]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        void OnEnable()
        {
            Window = this;
            PrefabBrowserIcon = PrefabBrowserIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/prefabBrowser") as Texture2D;
            SaveIcon = SaveIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/saveIcon") as Texture2D;
            LoadIcon = LoadIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/loadIcon") as Texture2D;
            NewSceneIcon = NewSceneIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/newSceneIcon") as Texture2D;
            CombatIcon = CombatIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/combatIcon") as Texture2D;
            DialogIcon = DialogIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/dialogIcon") as Texture2D;
            EventIcon = EventIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/eventsIcon") as Texture2D;
            AchievementIcon = AchievementIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/achievementIcon") as Texture2D;
            MapIcon = MapIcon ?? Resources.Load("RPGMakerAssets/ToolbarIcons/mapIcon") as Texture2D;
        }

        void OnGUI()
        {
            try
            {
                OnGUIx();
            }
            catch (Exception e)
            {
                Debug.Log("Editor Error: " + e.Message + "@" + e.Source);
            }
        }

        void Update()
        {
            this.Repaint();
        }

        private void OnGUIx()
        {
            GUI.skin = Resources.Load("RPGMakerAssets/EditorSkinRPGMaker") as GUISkin;
            if(Window.position.width > 600)
            {
                GUILayout.BeginHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();
            }

            if (GUILayout.Button(new GUIContent("RPGAIO", RPGMakerGUI.RPGMakerIcon), "rpgToolBarButton"))
            {
                Rme_Main.Init();
            }
            if (GUILayout.Button(new GUIContent("Prefab Window", PrefabBrowserIcon), "rpgToolBarButton"))
            {
                Rme_Tools_PrefabRepository.Init();
            }
            if (GUILayout.Button(new GUIContent("Save Data", SaveIcon), "rpgToolBarButton"))
            {
                EditorGameDataSaveLoad.SaveGameData();
            }
            if (GUILayout.Button(new GUIContent("Reload Data", LoadIcon), "rpgToolBarButton"))
            {
                EditorGameDataSaveLoad.LoadGameDataFromEditor();
            }

            if (GUILayout.Button(new GUIContent("Add New Scene", NewSceneIcon), "rpgToolBarButton"))
            {
                AddScene();
            }

            if (GUILayout.Button(new GUIContent("Combat", CombatIcon), "rpgToolBarButton"))
            {
                CombatNodeWindow.Init();
            }
            if (GUILayout.Button(new GUIContent("Dialog", DialogIcon), "rpgToolBarButton"))
            {
                DialogNodeWindow.Init();
            }
            if (GUILayout.Button(new GUIContent("Events", EventIcon), "rpgToolBarButton"))
            {
                EventNodeWindow.Init();
            }
            if (GUILayout.Button(new GUIContent("Achievements", AchievementIcon), "rpgToolBarButton"))
            {
                AchievementNodeWindow.Init();
            }
            if (GUILayout.Button(new GUIContent("Map", MapIcon), "rpgToolBarButton"))
            {
                WorldMapNodeWindow.Init();
            }

            GUILayout.EndHorizontal();
        }

        private static void AddScene()
        {
            var original = EditorBuildSettings.scenes; 
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);

            EditorApplication.NewScene();

            var e1 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/Essentials"), Vector3.zero, Quaternion.identity);
            var e2 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/Essentials_UI"), Vector3.zero, Quaternion.identity);
            var e3 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/SpawnPosition"), Vector3.zero, Quaternion.identity);

            e1.name = "[Essentials]";
            e2.name = "[Essentials_UI]";
            e3.name = "[Essentials_SpawnPosition]";



            var path = EditorUtility.SaveFilePanel("Scene Save Location","", "","unity");

            var dataPath = Application.dataPath;
            var shortenedPath = path.Replace(dataPath, "");
            shortenedPath = shortenedPath.Substring(1, shortenedPath.Length - 1);

            EditorApplication.SaveScene("Assets/" + shortenedPath, false);
            EditorApplication.OpenScene(shortenedPath);

            Debug.Log(path);
            if (path.Length != 0)
            {
                var sceneToAdd = new EditorBuildSettingsScene(path, true);
                newSettings[newSettings.Length - 1] = sceneToAdd;
                EditorBuildSettings.scenes = newSettings;
            }
        }

        private static void AddSceneEssentials()
        {
            var e1 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/Essentials"), Vector3.zero, Quaternion.identity);
            var e2 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/Essentials_UI"), Vector3.zero, Quaternion.identity);
            var e3 = (GameObject)Instantiate(Resources.Load("RPGMakerAssets/SpawnPosition"), Vector3.zero, Quaternion.identity);

            e1.name = "[Essentials]";
            e2.name = "[Essentials_UI]";
            e3.name = "[Essentials_SpawnPosition]";

            EditorUtility.DisplayDialog("Essentials Added.", "You will need to manually add the scene to the build scenes.", "OK");
        }

        public Rect PadRect(Rect rect, int left, int top)
        {
            return new Rect(rect.x + left, rect.y + top, rect.width - (left*2), rect.height - (top*2));
        }

    
    }
}
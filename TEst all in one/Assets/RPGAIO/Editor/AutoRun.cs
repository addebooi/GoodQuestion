using UnityEditor;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Editor
{
    [InitializeOnLoad]
    public class Autorun
    {
        private static bool loadedData = false;
    
        static Autorun()
        {
            EditorApplication.update += RunOnce;
            EditorApplication.update += UpdateData;
            EditorApplication.update += AutoSave;
        }

        private static void UpdateData()
        {
            Rm_DataUpdater.ScanAndUpdateData();
        }

        private static void AutoSave()
        {
            Rm_DataUpdater.AutoSave();
        }

        private static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            if(EditorGameDataSaveLoad.LoadIfNotLoadedFromEditor())
            {
                Debug.Log("[RPGMaker] Loading Game Data");
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void DidReloadScripts()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                Debug.Log("RPGMaker: Compiled scripts, reloading game data");
                EditorGameDataSaveLoad.LoadGameDataFromEditor();
                loadedData = true;
            }
        }
    }
}
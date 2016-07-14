using System.Collections.Generic;
using LogicSpawn.RPGMaker.Core;
using UnityEngine;

namespace LogicSpawn.RPGMaker
{
    public class Rmh_Customise
    {
        //Game
        public bool SaveInMenu;
        public bool SaveOnSceneSwitch;

        public bool PersistGameObjectInfo;
        public bool SaveEnemyStatus;
        public bool SaveGameObjectPosition;
        public bool SaveGameObjectRotation;
        public bool SaveGameObjectDestroyed;
        public bool SaveGameObjectEnabled;

        public bool GameHasAchievements;
        public AudioContainer AchievementUnlockedSound;
        public List<WorldArea> WorldMapLocations;
        public ImageContainer LoadingScreen;

        public Rmh_Customise()
        {
            SaveInMenu = true;
            SaveOnSceneSwitch = true;

            PersistGameObjectInfo = true;
            SaveEnemyStatus = true;
            SaveGameObjectPosition = true;
            SaveGameObjectRotation = true;
            SaveGameObjectDestroyed = true;
            SaveGameObjectEnabled = true;
            WorldMapLocations = new List<WorldArea>();
            GameHasAchievements = true;
            AchievementUnlockedSound = new AudioContainer();
            LoadingScreen = new ImageContainer();
        }
    }

    public class LayerMaskObject : ScriptableObject
    {
        public LayerMask Mask;

        public LayerMaskObject()
        {
            Mask = new LayerMask();
        }
    }
}
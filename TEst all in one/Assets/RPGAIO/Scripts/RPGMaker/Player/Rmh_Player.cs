using System.Collections.Generic;
using LogicSpawn.RPGMaker.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker
{
    public class Rmh_Player
    {
        //Player
        public List<Rm_ClassDefinition> ClassDefinitions;
        public List<DifficultyDefinition> Difficulties;
        public string DefaultDifficultyID;



        public bool ManualAssignAttrPerLevel;
        public int AttributePointsEarnedPerLevel;

        public AudioContainer LevelUpSound;


        public Rmh_Player()
        {
            ClassDefinitions = new List<Rm_ClassDefinition>();
            LevelUpSound = new AudioContainer();
            ManualAssignAttrPerLevel = true;
            AttributePointsEarnedPerLevel = 25;

            Difficulties = new List<DifficultyDefinition>()
                               {
                                   new DifficultyDefinition("Easy", 0.5f),
                                   new DifficultyDefinition("Normal", 1.0f),
                                   new DifficultyDefinition("Hard", 2.0f),
                                   new DifficultyDefinition("Expert", 3.0f)
                               };

            DefaultDifficultyID = Difficulties[1].ID;
        }
    }
}
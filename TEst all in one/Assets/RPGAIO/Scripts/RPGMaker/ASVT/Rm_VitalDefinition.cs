using System;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker
{
    public class Rm_VitalDefinition
    {
        public string Name = "Unnamed Vital";
        public int DefaultValue;
        public int UpperLimit = 100;
        public bool IsHealth;
        public string ID;
        public Rm_UnityColors Color = Rm_UnityColors.None;
        public bool HasUpperLimit;
        public float BaseRegenPercentValue;

        [JsonIgnore]
        public Texture2D _image ;
        [JsonIgnore]
        public Texture2D Image
        {
            get { return _image ?? (_image = Resources.Load(ImagePath) as Texture2D); }
            set { _image = value; }
        }
        public string ImagePath;

        public Rm_VitalDefinition()
        {
            ID = Guid.NewGuid().ToString();
            BaseRegenPercentValue = 0;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
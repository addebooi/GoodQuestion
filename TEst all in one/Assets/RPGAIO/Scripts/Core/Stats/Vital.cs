using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class Vital
    {
        public string ID ;
        public int BaseValue ;
        public int ScaledBaseValue;
        public int AttributeValue ;
        public int SkillValue ;
        public int EquipValue ;
        
        public float RegenPercentValue ;
        public bool IsHealth ;

        public bool HasUpperLimit;
        public int UpperLimit;

        public Rm_UnityColors Color;

        public int _currentValue;

        [JsonIgnore]
        public int CurrentValue
        {
            get
            {
                if ( _currentValue <= MaxValue) return _currentValue;

                _currentValue = MaxValue;
                return _currentValue;
            }
            set
            {
                _currentValue = value;
            }
        }

        public Vital()
        {

        }

        [JsonIgnore]
        public int MaxValue
        {
            get
            {
                var totalValue = ScaledBaseValue + AttributeValue + EquipValue + SkillValue;
                if(!HasUpperLimit)
                    return totalValue;

                return Mathf.Min(UpperLimit, totalValue);
            }
        }

        

        public void Reset()
        {
            AttributeValue = SkillValue = EquipValue = 0;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    //TODO:
    public class VitalHandler
    {
        [JsonIgnore]
        public BaseCharacter Character ;
        [JsonIgnore]
        public Vital Health
        {
            get { return Character.Vitals.First(v => v.IsHealth); }
        }

        public VitalHandler(BaseCharacter character)
        {
            Character = character;
        }

        public DamageOutcome TakeDamage(BaseCharacter attacker, Damage damage, bool evaluateDamageToDeal = true)
        {
            if (attacker != null && attacker.IsFriendly(Character))
            {
                return new DamageOutcome(new Damage() { MinDamage = 0, MaxDamage = 0, ElementalDamages = new List<ElementalDamage>() }, new DamageDealt(0, new Dictionary<string, int>()), AttackOutcome.FriendlyFire, false);    
            }

            if(Character.ImmuneTo(damage.SkillMetaID))
            {
                return new DamageOutcome(new Damage() { MinDamage = 0, MaxDamage = 0, ElementalDamages = new List<ElementalDamage>() }, new DamageDealt(0, new Dictionary<string, int>()), AttackOutcome.Immune, false);    
            }

            Damage damageToDeal;
            DamageOutcome result = null;
            if(evaluateDamageToDeal)
            {
                result = CombatCalcEvaluator.Instance.Evaluate(attacker, Character, damage);
                damageToDeal = result.DamageToDeal;
            }
            else
            {
                damageToDeal = damage;
            }

            if (!string.IsNullOrEmpty(damageToDeal.SkillMetaID))
            {
                var susceptibility = Character.AllSusceptibilites.Where(s => s.ID == damageToDeal.SkillMetaID).Sum(s => s.AdditionalDamage);
                var multiplier = Mathf.Max(0, 1 + susceptibility);
                damageToDeal.ApplyMultiplier(multiplier);
            }

            var damageDealt = damageToDeal.DamageDealt;

            if (attacker != null)
            {
                //Debug.Log(Character.Name + " took " + damageDealt + " from " + attacker.Name);
            }

            Health.CurrentValue -= damageDealt.Total;
            var killedTarget = false;

            if (Health.CurrentValue <= 0)
            {
                Health.CurrentValue = 0;
                Character.Alive = false;
                killedTarget = true;
            }

            return new DamageOutcome(damageToDeal, damageDealt, AttackOutcome.Success, killedTarget);    
        }

        public void IncreaseHealth(int value)
        {
            Health.CurrentValue += value;
        }

        public void ReduceHealth(int value)
        {
            Health.CurrentValue -= value;
        }

        //todo:
        public DamageOutcome TakeDoTDamage(BaseCharacter attacker, DamageOverTime dot)
        {
            var dotDamage = GeneralMethods.CopyObject(dot);
            dotDamage.DamagePerTick.SkillMetaID = dotDamage.SkillMetaID;

            if(attacker != null)
            {
                var outcome =  TakeDamage(attacker, dotDamage.DamagePerTick);
                return outcome;
            }

            //todo: test a dot with no attacker
            var outcomex =  TakeDamage(null, dotDamage.DamagePerTick, false);
            Debug.Log(Character.Name + " took " + outcomex.DamageDealt + " from " + dot.DoTName);
            return outcomex;
        }

        public DamageOutcome TakeFallDamage(float timeInAir)
        {
            var damageToTake = Health.MaxValue * 0.1f * timeInAir;  //todo: Rm_RPGHandler.Instance.Combat.FallDamagePerSec;
            var damage = new Damage();
            damage.MinDamage = damage.MaxDamage = (int)damageToTake;
            var outcome = TakeDamage(null, damage, false);
            Debug.Log(Character.Name + " lost " + outcome.DamageDealt.Total + "HP from fall damage");
            return outcome;
        }
    }
}
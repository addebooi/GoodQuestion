using System;
using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker.API;
using LogicSpawn.RPGMaker.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class PlayerCharacter : BaseCharacter
    {
        public string PlayerClassID ;
        public int Exp ;
        [JsonIgnore]
        public int ExpToLevel
        {
            get { return Rm_RPGHandler.Instance.Experience.ExpToNextPlayerLevel(ExpDefinitionID, Level); }
        }
        public int MaxLevel
        {
            get { return Rm_RPGHandler.Instance.Experience.Get(ExpDefinitionID).MaxLevel; }
        }
        public Equipment Equipment ;
        public Inventory Inventory ;
        public List<Trait> Traits ;
        public TalentHandler TalentHandler ;
        public SkillHandler SkillHandler;
        public new int Level = 1;
        public int CurrentSkillPoints = 0;
        public int CurrentTalentPoints = 0;
        public string ExpDefinitionID;

        [JsonConstructor]
        public PlayerCharacter()
        {
        }

        public PlayerCharacter(string classID, string name)
        {
            CharacterType = CharacterType.Player;
            PlayerClassID = classID;

            AttackStyle = AttackStyle.Melee;
            Name = name;
            Exp = 0;
            Equipment = new Equipment(this);
            Inventory = new Inventory {Player = this};
            Traits = new List<Trait>();
            TalentHandler = new TalentHandler(PlayerClassID);
            SkillHandler = new SkillHandler(this);
            for (var cnt = 0; cnt < Rm_RPGHandler.Instance.ASVT.TraitDefinitions.Count; cnt++) //Hard-coded number of traits
            {
                Traits.Add(
                    new Trait
                        {
                            ID = Rm_RPGHandler.Instance.ASVT.TraitDefinitions[cnt].ID,
                            Exp = 0,
                            Level = Rm_RPGHandler.Instance.ASVT.TraitDefinitions[cnt].StartingLevel,
                            ExpDefinitionID = Rm_RPGHandler.Instance.ASVT.TraitDefinitions[cnt].ExpDefinitionID
                        });
            }
        }

        public void AddSkillPoints(int skillPointsGained)
        {
            CurrentSkillPoints += skillPointsGained;
        }

        public void AddTraitExp(string traitId, int expGained)
        {
            var trait = GetTraitByID(traitId);
            if(trait != null)
            {
                trait.AddExp(expGained);
            }
            else
            {
                Debug.LogError("Trait not found with id:" + traitId);
            }
        }

        public bool AddExp(int amount)
        {
            var currentLevel = Level;

            Exp += amount;

            CheckLevelUp();

            var leveled = currentLevel < Level;
            if (leveled)
            {

                RPG.Popup.ShowInfo("Level " + Level + " Reached!");
                HandleLevelUp();
            }

            var args = new RPGEvents.GainedExpEventArgs {ExpGained = amount, Leveled = leveled};
            RPG.Events.OnGainedExp(args);
            RPG.Popup.ShowInfo("+" + Exp + " Exp");

            return leveled;
        }

        public override  void Init()
        {
            base.Init();


            var classDef = Rm_RPGHandler.Instance.Player.ClassDefinitions.First(c => c.ID == PlayerClassID);

            var startingGold = classDef.StartingGold;
            Inventory.Gold = startingGold;

            var immunities = classDef.SkillMetaImmunitiesID;
            for (int i = 0; i < immunities.Count; i++)
            {
                var immun = immunities[i];
                SkillMetaImmunitiesID.Add(new SkillImmunity()
                                              {
                                                  HasDuration = false,
                                                  ID = immun
                                              });
            }

            var susceptibilities = classDef.SkillMetaSusceptibilities;
            for (int i = 0; i < susceptibilities.Count; i++)
            {
                var sus = susceptibilities[i];
                SkillMetaSusceptibilities.Add(new SkillMetaSusceptibility()
                                              {
                                                  HasDuration = false,
                                                  ID = sus.ID,
                                                  AdditionalDamage = sus.AdditionalDamage
                                              });
            }

            var startingSkills = classDef.StartingSkillIds;
            for (int i = 0; i < SkillHandler.AvailableSkills.Count; i++)
            {
                var x = SkillHandler.AvailableSkills[i];
                if(startingSkills.FirstOrDefault(s => s == x.ID) != null)
                {
                    x.Unlocked = true;
                    x.Learnt = true;
                    SkillHandler.Slots[i].ChangeSlotTo(x);    

                }
            }

            var startingTalents = classDef.StartingTalentIds;
            for (int i = 0; i < TalentHandler.Talents.Count; i++)
            {
                var x = TalentHandler.Talents[i];
                if (startingTalents.FirstOrDefault(s => s == x.ID) != null)
                {
                    x.IsActive = true;
                    x.Learnt = true;
                }
            }


            var startingItems = classDef.StartingItems;
            for (int i = 0; i < startingItems.Count; i++)
            {
                var x = startingItems[i];
                var item = x.ItemID;
                var amount = x.Amount;

                var itemObj = Rm_RPGHandler.Instance.Repositories.Items.Get(item);
                var stackable = itemObj as IStackable;
                if (stackable != null)
                {
                    stackable.CurrentStacks = amount;
                }

                if(!Inventory.AddItem(itemObj))
                {
                    Debug.LogWarning("Failed adding starting item to inventory, it may be full or not have enough space.");
                }
            }

            var startingEquippedItems = classDef.StartingEquipped;
            for (int i = 0; i < startingEquippedItems.Count; i++)
            {
                var x = startingEquippedItems[i];
                if (!x.Enabled) continue;

                var item = x.ItemID;
                var itemObj = Rm_RPGHandler.Instance.Repositories.Items.Get(item);
                var apparel = itemObj as Apparel;
                if(apparel != null)
                {
                    var slot = Equipment.GetSlot(apparel.apparelSlotID);
                    if (slot == null) continue;

                    slot.Item = itemObj;
                }
                else
                {
                    var slot = Equipment.GetSlot("OffHand");
                    if (slot == null) continue;
                    slot.Item = itemObj;
                }
                
            }

            var startingWeapon = classDef.StartingEquippedWeapon;
            if (startingWeapon.Enabled)
            {
                var item = startingWeapon.ItemID;
                var itemObj = Rm_RPGHandler.Instance.Repositories.Items.Get(item);

                var slot = Equipment.GetSlot("Weapon");
                if (slot != null)
                {
                    slot.Item = itemObj;
                }
            }
        }

        public void AddProgression(ProgressionGain progressionGain)
        {
            if (progressionGain.GainExp)
            {
                if(progressionGain.GainExpWithDefinition)
                {
                    var expDefinition = Rm_RPGHandler.Instance.Experience.AllExpDefinitions.FirstOrDefault(e => e.ID == progressionGain.GainExpWithDefinitionID);
                    if(expDefinition != null)
                    {
                        var expToAward=  expDefinition.ExpForLevel(progressionGain.CombatantLevel);
                        AddExp(expToAward);
                    }
                    else
                    {
                        Debug.LogError("Could not find exp definition to award exp. ID:" + progressionGain.GainExpWithDefinition);
                    }
                }
                else
                {
                    AddExp(progressionGain.ExpGained);    
                }
            }
            if (progressionGain.GainSkillPoints)
            {
                if (progressionGain.GainSkillWithDefinition)
                {
                    var expDefinition = Rm_RPGHandler.Instance.Experience.AllExpDefinitions.FirstOrDefault(e => e.ID == progressionGain.GainSkillWithDefinitionID);
                    if (expDefinition != null)
                    {
                        var expToAward = expDefinition.ExpForLevel(progressionGain.CombatantLevel);
                        AddSkillPoints(expToAward);
                    }
                    else
                    {
                        Debug.LogError("Could not find exp definition to award skill points. ID:" + progressionGain.GainExpWithDefinition);
                    }
                }
                else
                {
                    AddSkillPoints(progressionGain.SkillPointsGained);
                }
            }
            if (progressionGain.GainTraitExp)
            {
                if (progressionGain.GainTraitWithDefinition)
                {
                    var expDefinition = Rm_RPGHandler.Instance.Experience.AllExpDefinitions.FirstOrDefault(e => e.ID == progressionGain.GainTraitWithDefinitionID);
                    if (expDefinition != null)
                    {
                        var expToAward = expDefinition.ExpForLevel(progressionGain.CombatantLevel);
                        AddTraitExp(progressionGain.TraitID, expToAward);
                    }
                    else
                    {
                        Debug.LogError("Could not find exp definition to award trait exp. ID:" + progressionGain.GainExpWithDefinition);
                    }
                }
                else
                {
                    AddTraitExp(progressionGain.TraitID, progressionGain.TraitExpGained);
                }
            }
        }

        private void CheckLevelUp()
        {
            if (Exp >= ExpToLevel && Level < MaxLevel)
            {
                Exp -= ExpToLevel;
                Level++;
            }
        }

        private void HandleLevelUp()
        {
            var levelUpSound = Rm_RPGHandler.Instance.Player.LevelUpSound;
            AudioPlayer.Instance.Play(levelUpSound.Audio, AudioType.SoundFX,Vector3.zero);

            if(!Rm_RPGHandler.Instance.Player.ManualAssignAttrPerLevel)
            {
                var definition = Rm_RPGHandler.Instance.Player.ClassDefinitions.First(c => c.ID == PlayerClassID).AttributePerLevel;
                foreach (var scaling in definition)
                {
                    GetAttributeByID(scaling.AsvtID).BaseValue += (int)scaling.Amount;
                }
   
            }
            else
            {
                CurrentSkillPoints += Rm_RPGHandler.Instance.Player.AttributePointsEarnedPerLevel;
            }

            var skillsToUnlock = SkillHandler.AvailableSkills.Where(s => s.AutomaticallyUnlockAtLevel && !s.Unlocked && Level >= s.LevelToAutomaticallyUnlock).ToList();
            var talentsToUnlock = TalentHandler.Talents.Where(s => s.AutomaticallyUnlockAtLevel && !s.Learnt && Level >= s.LevelToAutomaticallyUnlock).ToList();

            foreach(var skill in skillsToUnlock)
            {
                skill.Unlocked = true;
            }
            foreach (var talent in talentsToUnlock)
            {
                talent.Learnt = true;
                if(talent.CanToggle)
                {
                    talent.IsActive = true;
                }
            }

            FullUpdateStats();
        }

        protected internal void ApplyEquipmentStats()
        {
            var equippedItems = Equipment.AllEquippedItems;
            foreach (var item in equippedItems)
            {
                if (item == null) continue;

                var equippedItem = item as BuffItem;

                if (equippedItem == null) continue;
                foreach (var ab in equippedItem.AttributeBuffs)
                {
                    GetAttributeByID(ab.AttributeID).EquipValue += ab.Amount;
                }

                foreach (var vb in equippedItem.VitalBuffs)
                {
                    GetVitalByID(vb.VitalID).EquipValue += vb.Amount;
                }

                foreach (var stat in equippedItem.StatisticBuffs)
                {
                    GetStatByID(stat.StatisticID).EquipValue += stat.Amount;
                }
            }


            var weapon = Equipment.EquippedWeapon as Weapon;
            weapon = weapon ?? (Rm_RPGHandler.Instance.Items.EnableOffHandSlot ? Equipment.EquippedOffHand as Weapon : null);
            if (weapon != null)
            {
                GetStatByID("Attack Range").BaseValue = 0;
                GetStatByID("Attack Speed").BaseValue = 0;

                var baseWepType = Rm_RPGHandler.Instance.Items.WeaponTypes.First(w => w.ID == weapon.WeaponTypeID);
                var baseWepRange = baseWepType.AttackRange;
                var baseWepSpeed = baseWepType.AttackSpeed;


                var wepRange = baseWepRange;
                var wepAttackSpeed = baseWepSpeed;

                if(weapon.OverrideAttackRange)
                {
                    wepRange = weapon.NewAttackRange;
                }
                if(weapon.OverrideAttackSpeed)
                {
                    wepAttackSpeed = weapon.NewAttackSpeed;
                }

                

                GetStatByID("Attack Range").EquipValue += wepRange;
                GetStatByID("Attack Speed").EquipValue += wepAttackSpeed;
            }
            else
            {
                GetStatByID("Attack Range").BaseValue = Rm_RPGHandler.Instance.Player.ClassDefinitions.First(c => c.ID == PlayerClassID).UnarmedAttackRange;
                GetStatByID("Attack Speed").BaseValue = Rm_RPGHandler.Instance.Player.ClassDefinitions.First(c => c.ID == PlayerClassID).UnarmedAttackSpeed;
            }
        }
        
        protected internal void ApplyTalents()
        {
            TalentHandler.ApplyTalents();
        }

        public Trait GetTraitByID(string traitId)
        {
            return Traits.FirstOrDefault(s => s.ID == traitId);
        }

        public Trait GetTrait(string trait)
        {
            var id = RPG.Stats.GetTraitId(trait);
            return id != null ? GetTraitByID(id) : null;
        }

        public bool ToggleTalent(Talent talent)
        {
            bool active;
            if (!talent.IsActive)
            {
                talent.IsActive = true;
                CustomVariableHandler.HandleCvarSetters(talent.TalentEffect.Effect.CustomVariableSetters);
                active = true;
            }
            else
            {
                talent.IsActive = false;
                CustomVariableHandler.HandleCvarSetters(talent.TalentEffect.Effect.CustomVariableSettersOnDisable);
                DeactivatePassive(talent.TalentEffect.Effect);
                active = false;
            }

            FullUpdateStats();
            return active;
        }


        public void InitStatsFromNewSave()
        {
            //Add proc effects from Auras, Buffs, Debuffs and Talents
            ResetStats();

            ApplyEquipmentStats();

            //ScaleStats(); //todo: allow this
            ScaleStatsCC();
            ApplyTalents();

            ApplyAuras();
            ApplyBuffDebuffs();
            ApplyStatusEffects();
            DetermineState();

            _damageDealable = CombatCalcEvaluator.EvaluateDamageDealt(this);
        }
    }
}
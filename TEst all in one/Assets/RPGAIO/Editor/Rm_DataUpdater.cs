using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker.Core;
using LogicSpawn.RPGMaker.Editor.New;
using UnityEngine;


namespace LogicSpawn.RPGMaker
{
    public static class Rm_DataUpdater
    {
        private static float AutoSaveTime = 0;

        public static void ScanAndUpdateData()
        {
            if (Rm_RPGHandler.Instance == null) return;

            UpdateInteractables();
            UpdateCombatants();
            UpdateNodes();
            UpdateItemInfo();
        }

        private static void UpdateItemInfo()
        {
            foreach(var i in Rm_RPGHandler.Instance.Items.CraftSlotScalings)
            {
                var slot = Rm_RPGHandler.Instance.Items.ApparelSlots.FirstOrDefault(a => a.ID == i.SlotIdentifier);
                if(slot != null)
                {
                    i.SlotName = slot.Name;
                }
            }
        }

        private static void UpdateInteractables()
        {

            //Update harvestable animations
            foreach (var def in Rm_RPGHandler.Instance.Harvesting.HarvestableDefinitions)
            {
                var anims = def.ClassHarvestingAnims;
                foreach (var d in Rm_RPGHandler.Instance.Player.ClassDefinitions)
                {
                    var classDef = anims.FirstOrDefault(a => a.ClassID == d.ID );
                    if (classDef == null)
                    {
                        var classAnimToAdd = new ClassAnimationDefinition()
                        {
                            ClassID = d.ID,
                            LegacyAnim = ""
                        };

                        anims.Add(classAnimToAdd);
                    }
                }

                for (int index = 0; index < anims.Count; index++)
                {
                    var v = anims[index];
                    var stillExists =
                        Rm_RPGHandler.Instance.Player.ClassDefinitions.FirstOrDefault(
                            a => a.ID == v.ClassID);

                    if (stillExists == null)
                    {
                        anims.Remove(v);
                        index--;
                    }
                }
            }
            
                
        }

        private static void UpdateNodes()
        {
            var damageDealtTree = Rm_RPGHandler.Instance.Nodes.DamageDealtTree;
            var statScalingTree = Rm_RPGHandler.Instance.Nodes.StatScalingTree;
            var vitalScalingTree = Rm_RPGHandler.Instance.Nodes.VitalScalingTree;
            var worldNodeBank = Rm_RPGHandler.Instance.Nodes.WorldMapNodeBank;

            //delete non existing world areas
            for (int index = 0; index < worldNodeBank.NodeTrees.Count; index++)
            {
                var w = worldNodeBank.NodeTrees[index];
                var existingWorldArea = Rm_RPGHandler.Instance.Customise.WorldMapLocations.FirstOrDefault(a => a.ID == w.ID);
                if (existingWorldArea == null)
                {
                    worldNodeBank.NodeTrees.RemoveAt(index);
                    index--;
                }
            }

            //locations
            foreach(var w in worldNodeBank.NodeTrees)
            {
                var existingWorldArea = Rm_RPGHandler.Instance.Customise.WorldMapLocations.First(a => a.ID == w.ID);
                //delete non-existing locations
                for (int index = 0; index < w.Nodes.Count; index++)
                {
                    var n = w.Nodes[index];
                    var existingLocation = existingWorldArea.Locations.FirstOrDefault(a => a.ID == n.ID);
                    if (existingLocation == null)
                    {
                        w.Nodes.RemoveAt(index);
                        index--;
                    }
                }
            }

            //add non-existing world locations
            foreach(var wa in Rm_RPGHandler.Instance.Customise.WorldMapLocations)
            {
                var waTree = worldNodeBank.NodeTrees.FirstOrDefault(a => a.ID == wa.ID);
                if(waTree != null)
                {
                    foreach(var loc in wa.Locations)
                    {
                        var locNode = waTree.Nodes.FirstOrDefault(a => a.ID == loc.ID);
                        if(locNode == null)
                        {
                            var nodePos = Vector2.zero;
                            var nodeToAdd = new LocationNode();
                            var nextId = updateNode_GetNextID(worldNodeBank);
                            waTree.AddNode(nodeToAdd, nodePos, nextId, loc.ID);
                        }
                        else
                        {
                            var locationNode = locNode as LocationNode;
                            locationNode.LocName = loc.Name;
                        }
                    }
                }
            }

            //update node tree var variable names
            foreach(var wa in Rm_RPGHandler.Instance.Nodes.AllTrees)
            {
                var s = wa.Nodes.OfType<NodeTreeVarNode>().ToList();
                for (int i = 0; i < s.Count(); i++)
                {
                    var n = s[i];
                    var treeVar = wa.Variables.FirstOrDefault(v => v.ID == n.VariableId);
                    if (treeVar == null)
                    {
                        wa.Nodes.Remove(n);
                        continue;
                    }
                    n.NewName = treeVar.Name;
                }
            }

            //todo: update min max damage dealt node names

            //Delete damage dealt nodes
            var nodesToDelete = damageDealtTree.Nodes.Where(
                n => n.GetType() == typeof(CombatStartNode) && n.Identifier != "MIN_Physical" && n.Identifier != "MAX_Physical" 
                    && Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.All(e => ("MIN_" + e.ID) != n.Identifier)
                    && Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.All(e => ("MAX_" + e.ID) != n.Identifier)
                ).ToList();

            for (int index = 0; index < nodesToDelete.Count; index++)
            {
                var delete = nodesToDelete.ToList()[index];
                updateNode_DeleteNode(delete, damageDealtTree);
            }

            //Delete skill scaling nodes
            var skillsWithScaling = Rm_RPGHandler.Instance.Repositories.Skills.AllSkills.Where(s => !string.IsNullOrEmpty(s.DamageScalingTreeID)).ToList();
            for (int i = 0; i < skillsWithScaling.Count; i++)
            {
                var skillWithScaling = skillsWithScaling[i];
                var tree = Rm_RPGHandler.Instance.Nodes.CombatNodeBank.NodeTrees.FirstOrDefault(n => n.ID == skillWithScaling.DamageScalingTreeID);
                nodesToDelete = tree.Nodes.Where(n => n.GetType() == typeof (CombatStartNode) && n.Identifier != "Physical" && Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.All(e => e.ID != n.Identifier)).ToList();
                for (int index = 0; index < nodesToDelete.Count; index++)
                {
                    var delete = nodesToDelete.ToList()[index];
                    updateNode_DeleteNode(delete, tree);
                }
            }

            
            nodesToDelete = statScalingTree.Nodes.Where(n => n.GetType() == typeof (CombatStartNode) && Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.All(e => e.ID != n.Identifier)).ToList();
            for (int index = 0; index < nodesToDelete.Count; index++)
            {
                var delete = nodesToDelete.ToList()[index];
                updateNode_DeleteNode(delete, statScalingTree);
            }

            nodesToDelete = vitalScalingTree.Nodes.Where(n => n.GetType() == typeof(CombatStartNode) && Rm_RPGHandler.Instance.ASVT.VitalDefinitions.All(e => e.ID != n.Identifier)).ToList();
            for (int index = 0; index < nodesToDelete.Count; index++)
            {
                var delete = nodesToDelete.ToList()[index];
                updateNode_DeleteNode(delete, vitalScalingTree);
            }
            
            //damage dealt: add non-existing element damages
            var notAdded = Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.Where(c => damageDealtTree.Nodes.All(n => n.Identifier != "MAX_" + c.ID)).ToList();
            var existingAmt = Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.Count - notAdded.Count;
            for (int index = 0; index < notAdded.Count; index++)
            {
                var element = notAdded[index];

                if(Rm_RPGHandler.Instance.Items.DamageHasVariance)
                {
                    var newNode = new CombatStartNode("Min \"" + element.Name + "\" Damage", "Min Damage for " + element.Name, "Damage dealt by an element starts at 0. You can add additional calculations to increase this amount. Example: Create a statistic called fire damage and add this amount.") { Identifier = "MIN_" + element.ID };
                    newNode.WindowID = updateNode_GetNextID(Rm_RPGHandler.Instance.Nodes.CombatNodeBank);
                    newNode.Rect = new Rect(25, 250 + ((existingAmt + index) * 200), 50, 50);
                    newNode.OnCreate();
                    damageDealtTree.Nodes.Add(newNode);
                }

                var newNodeMax = new CombatStartNode("Max \"" + element.Name + "\" Damage", "Max Damage for " + element.Name, "Damage dealt by an element starts at 0. You can add additional calculations to increase this amount. Example: Create a statistic called fire damage and add this amount.") { Identifier = "MAX_" + element.ID };
                newNodeMax.WindowID = updateNode_GetNextID(Rm_RPGHandler.Instance.Nodes.CombatNodeBank);
                newNodeMax.Rect = new Rect(25, 350 + ((existingAmt + index) * 200), 50, 50);
                newNodeMax.OnCreate();
                damageDealtTree.Nodes.Add(newNodeMax);
            }

            for (int i = 0; i < skillsWithScaling.Count; i++)
            {
                var skillWithScaling = skillsWithScaling[i];
                var tree = Rm_RPGHandler.Instance.Nodes.CombatNodeBank.NodeTrees.FirstOrDefault(n => n.ID == skillWithScaling.DamageScalingTreeID);
                if(tree != null)
                {
                    var notSkillAdded = Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.Where(c => tree.Nodes.All(n => n.Identifier != c.ID)).ToList();
                    var existingSkillAmt = Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.Count - notSkillAdded.Count;
                    for (int index = 0; index < notSkillAdded.Count; index++)
                    {
                        var element = notSkillAdded[index];
                        var newNode = new CombatStartNode(element.Name, "Damage for " + element.Name, "You can add additional calculations to increase this amount. Example: Create a statistic called fire damage and add this amount.") { Identifier = element.ID };
                        newNode.WindowID = updateNode_GetNextID(Rm_RPGHandler.Instance.Nodes.CombatNodeBank);
                        newNode.Rect = new Rect(25, 250 + ((existingSkillAmt + index) * 200), 50, 50);
                        newNode.OnCreate();
                        tree.Nodes.Add(newNode);
                    } 
                }
            }
            
            foreach (var x in damageDealtTree.Nodes.Where(n => n.GetType() == typeof(CombatStartNode) && n.Identifier != "Physical"))
            {
                var element = Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.FirstOrDefault(e => e.ID == x.Identifier);
                if (element != null)
                {
                    var n = (CombatStartNode)x;
                    n.NewName = element.Name;
                    n.NewSubText = "Damage for " + element.Name;    
                }
            } 
            
            //stat scaling: add non-existing stats
            var statsNotAdded = Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.Where(c => statScalingTree.Nodes.All(n => n.Identifier != c.ID)).ToList();
            var existingStatAmt = Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.Count - statsNotAdded.Count;
            for (int index = 0; index < statsNotAdded.Count; index++)
            {
                var element = statsNotAdded[index];
                var newNode = new CombatStartNode(element.Name, "Scaling for " + element.Name, "Add additional nodes to modify base value of a statistic based on other values, e.g. an attribute. Remember: don't cause circular scaling as this can result in an infinite loop.") { Identifier = element.ID };
                newNode.WindowID = updateNode_GetNextID(Rm_RPGHandler.Instance.Nodes.CombatNodeBank);
                newNode.Rect = new Rect(25, 87.5f + ((existingStatAmt + index) * 200), 50, 50);
                newNode.OnCreate();
                statScalingTree.Nodes.Add(newNode);
            }

            foreach (var x in statScalingTree.Nodes.Where(n => n.GetType() == typeof(CombatStartNode)))
            {
                var element = Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.FirstOrDefault(e => e.ID == x.Identifier);
                if (element != null)
                {
                    var n = (CombatStartNode)x;
                    n.NewName = element.Name;
                    n.NewSubText = "Scaling for " + element.Name;
                }
            }


            //stat scaling: add non-existing stats
            var vitNotAdded = Rm_RPGHandler.Instance.ASVT.VitalDefinitions.Where(c => vitalScalingTree.Nodes.All(n => n.Identifier != c.ID)).ToList();
            var existingVitAmt = Rm_RPGHandler.Instance.ASVT.VitalDefinitions.Count - vitNotAdded.Count;
            for (int index = 0; index < vitNotAdded.Count; index++)
            {
                var element = vitNotAdded[index];
                var newNode = new CombatStartNode(element.Name, "Scaling for " + element.Name, "Add additional nodes to modify base value of a vital based on other values, e.g. an attribute. Remember: don't cause circular scaling as this can result in an infinite loop.") { Identifier = element.ID };
                newNode.WindowID = updateNode_GetNextID(Rm_RPGHandler.Instance.Nodes.CombatNodeBank);
                newNode.Rect = new Rect(25, 87.5f + ((existingVitAmt + index) * 200), 50, 50);
                newNode.OnCreate();
                vitalScalingTree.Nodes.Add(newNode);
            }

            foreach (var x in vitalScalingTree.Nodes.Where(n => n.GetType() == typeof(CombatStartNode)))
            {
                var element = Rm_RPGHandler.Instance.ASVT.VitalDefinitions.FirstOrDefault(e => e.ID == x.Identifier);
                if (element != null)
                {
                    var n = (CombatStartNode)x;
                    n.NewName = element.Name;
                    n.NewSubText = "Scaling for " + element.Name;
                }
            }
        }

        private static int updateNode_GetNextID(NodeBank nodeBank)
        {
            return nodeBank.NodeTrees.SelectMany(n => n.Nodes).Any()
                       ? (nodeBank.NodeTrees.SelectMany(n => n.Nodes).Max(n => n.WindowID) + 1)
                       : 0;
        }
        private static void updateNode_ClearPrevLinks(Node node, NodeTree nodeTree)
        {
            foreach (var link in node.PrevNodeLinks)
            {
                var foundNode = nodeTree.Nodes.FirstOrDefault(n => n.ID == link.ID);
                if (foundNode != null)
                {
                    foundNode.NextNodeLinks.Where(n => n.ID == node.ID).ToList().ForEach(s => s.ID = "");
                }
            }

            node.PrevNodeLinks = new List<StringField>();
        }
        private static void updateNode_DeleteNode(Node nodeToDelete, NodeTree nodeTree)
        {
            updateNode_ClearPrevLinks(nodeToDelete, nodeTree);
            for (int i = 0; i < nodeToDelete.NextNodeLinks.Count; i++)
            {
                updateNode_RemoveLink(nodeToDelete, i, nodeTree);
            }

            nodeTree.Nodes.Remove(nodeToDelete);
        }
        private static void updateNode_RemoveLink(Node node, int linkIndex, NodeTree nodeTree)
        {
            var linkedNodeID = node.NextNodeLinks[linkIndex];
            var foundNode = nodeTree.Nodes.FirstOrDefault(n => n.ID == linkedNodeID.ID);
            if (foundNode != null)
            {
                var link = foundNode.PrevNodeLinks.FirstOrDefault(s => s.ID == node.ID);
                if (link != null)
                {
                    foundNode.PrevNodeLinks.Remove(link);
                }
            }
            linkedNodeID.ID = "";
        }

        private static void UpdateCombatants()
        {
            //Update enemies and npcs
            var enemiesAndnpcs = Rm_RPGHandler.Instance.Repositories.Enemies.AllEnemies.Concat(
                Rm_RPGHandler.Instance.Repositories.Interactable.AllNpcs.Select(n => n as CombatCharacter)
                ).ToList();

            for (int charIndex = 0; charIndex < enemiesAndnpcs.Count; charIndex++)
            {
                var selectedCharInfo = enemiesAndnpcs[charIndex];

                //Update Attributes
                foreach (var d in Rm_RPGHandler.Instance.ASVT.AttributesDefinitions)
                {
                    var attr = selectedCharInfo.Attributes.FirstOrDefault(a => a.ID == d.ID);
                    if (attr == null)
                    {
                        var attributeToAdd = new Attribute
                        {
                            ID = d.ID,
                            BaseValue = d.DefaultValue
                        };

                        selectedCharInfo.Attributes.Add(attributeToAdd);
                    }
                }

                for (int index = 0; index < selectedCharInfo.Attributes.Count; index++)
                {
                    var v = selectedCharInfo.Attributes[index];
                    var stillExists =
                        Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.FirstOrDefault(
                            a => a.ID == v.ID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.Attributes.Remove(v);
                        index--;
                    }
                }

                //Update Vitals
                foreach (var d in Rm_RPGHandler.Instance.ASVT.VitalDefinitions)
                {
                    var vital = selectedCharInfo.Vitals.FirstOrDefault(a => a.ID == d.ID);
                    if (vital == null)
                    {
                        var vitToAdd = new Vital
                        {
                            ID = d.ID,
                            BaseValue = d.DefaultValue
                        };

                        selectedCharInfo.Vitals.Add(vitToAdd);
                    }
                }

                for (int index = 0; index < selectedCharInfo.Vitals.Count; index++)
                {
                    var v = selectedCharInfo.Vitals[index];
                    var stillExists =
                        Rm_RPGHandler.Instance.ASVT.VitalDefinitions.FirstOrDefault(
                            a => a.ID == v.ID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.Vitals.Remove(v);
                        index--;
                    }
                }

                //Update statistics
                foreach (var d in Rm_RPGHandler.Instance.ASVT.StatisticDefinitions)
                {
                    var stat = selectedCharInfo.Stats.FirstOrDefault(a => a.ID == d.ID);
                    if (stat == null)
                    {
                        var statToAdd = new Statistic()
                        {
                            ID = d.ID,
                            BaseValue = d.DefaultValue
                        };

                        selectedCharInfo.Stats.Add(statToAdd);
                    }
                }

                for (int index = 0; index < selectedCharInfo.Stats.Count; index++)
                {
                    var v = selectedCharInfo.Stats[index];
                    var stillExists =
                        Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.FirstOrDefault(
                            a => a.ID == v.ID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.Stats.Remove(v);
                        index--;
                    }
                }

                //Update Immunities
                for (int index = 0; index < selectedCharInfo.SkillMetaImmunitiesID.Count; index++)
                {
                    var v = selectedCharInfo.SkillMetaImmunitiesID[index];
                    if (string.IsNullOrEmpty(v.ID)) continue;

                    var stillExists =
                        Rm_RPGHandler.Instance.Combat.SkillMeta.FirstOrDefault(
                            a => a.ID == v.ID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.SkillMetaImmunitiesID.Remove(v);
                        index--;
                    }
                }

                //Update Susceptibilities
                for (int index = 0; index < selectedCharInfo.SkillMetaSusceptibilities.Count; index++)
                {
                    var v = selectedCharInfo.SkillMetaSusceptibilities[index];
                    if (string.IsNullOrEmpty(v.ID)) continue;

                    var stillExists =
                        Rm_RPGHandler.Instance.Combat.SkillMeta.FirstOrDefault(
                            a => a.ID == v.ID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.SkillMetaSusceptibilities.Remove(v);
                        index--;
                    }
                }

                //Update skills
                for (int index = 0; index < selectedCharInfo.EnemySkills.Count; index++)
                {
                    var v = selectedCharInfo.EnemySkills[index];
                    if (string.IsNullOrEmpty(v.SkillID)) continue;

                    var stillExists =
                        Rm_RPGHandler.Instance.Repositories.Skills.AllSkills.FirstOrDefault(
                            a => a.ID == v.SkillID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.EnemySkills.Remove(v);
                        index--;
                    }
                }

                //Update damages
                var dmgList = selectedCharInfo.NpcDamage.ElementalDamages;
                foreach (var d in Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions)
                {
                    var tier = dmgList.FirstOrDefault(t => t.ElementID == d.ID);
                    if (tier == null)
                    {
                        var tierToAdd = new ElementalDamage() { ElementID = d.ID };
                        dmgList.Add(tierToAdd);
                    }
                }

                for (int index = 0; index < dmgList.Count; index++)
                {
                    var v = dmgList[index];
                    var stillExists =
                        Rm_RPGHandler.Instance.ASVT.ElementalDamageDefinitions.FirstOrDefault(
                            t => t.ID == v.ElementID);

                    if (stillExists == null)
                    {
                        dmgList.Remove(v);
                        index--;
                    }
                }

                //Update LootTables
                for (int index = 0; index < selectedCharInfo.LootTables.Count; index++)
                {
                    var v = selectedCharInfo.LootTables[index];
                    if (string.IsNullOrEmpty(v.LootTableID)) continue;

                    var stillExists =
                        Rm_RPGHandler.Instance.Repositories.LootTables.AllTables.FirstOrDefault(
                            a => a.ID == v.LootTableID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.LootTables.Remove(v);
                        index--;
                    }
                }

                //Update Loot
                for (int index = 0; index < selectedCharInfo.GuaranteedLoot.Count; index++)
                {
                    var v = selectedCharInfo.GuaranteedLoot[index];
                    if (string.IsNullOrEmpty(v.ItemID)) continue;

                    var stillExists =
                        Rm_RPGHandler.Instance.Repositories.Items.AllItems.FirstOrDefault(
                            a => a.ID == v.ItemID);

                    if (stillExists == null)
                    {
                        selectedCharInfo.GuaranteedLoot.Remove(v);
                        index--;
                    }
                }
            }
        }

        public static void AutoSave()
        {
            if (Rm_RPGHandler.Instance == null) return;

            if (!Rm_RPGHandler.Instance.Preferences.EnableAutoSave) return;
            if (Application.isPlaying) return;

            if(AutoSaveTime == 0)
            {
                if(Rm_RPGHandler.Instance != null)  
                {
                    InitAutoSave();
                }
                else
                {
                    return;
                }
            }

            if (Time.realtimeSinceStartup >= AutoSaveTime)
            {
                EditorGameDataSaveLoad.SaveGameData(true);
                Notify.Save("Autosaved Game Data");
                InitAutoSave();
            }
        }

        public static void InitAutoSave()
        {
            Rm_RPGHandler.Instance.Preferences.AutoSaveFrequency = Mathf.Max(Rm_RPGHandler.Instance.Preferences.AutoSaveFrequency, 30);
            AutoSaveTime = Time.realtimeSinceStartup + Rm_RPGHandler.Instance.Preferences.AutoSaveFrequency;
        }
    }
}
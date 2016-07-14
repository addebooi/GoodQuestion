using System;
using System.Collections.Generic;
using System.Linq;
using LogicSpawn.RPGMaker.Core;
using UnityEditor;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Editor
{
    public static class Rme_Main_Player
    {
        public static Rmh_Player Player
        {
            get { return Rm_RPGHandler.Instance.Player; }
        }
        #region Options

        private static int selectedStartDiffIndex;
        private static bool showDifficulties = true;
        private static Vector2 optionsScrollPos = Vector2.zero;
        public static void Options(Rect fullArea, Rect leftArea, Rect mainArea)
        {
            GUI.Box(fullArea, "", "backgroundBox");
            GUILayout.BeginArea(fullArea);
            optionsScrollPos = GUILayout.BeginScrollView(optionsScrollPos);
            RPGMakerGUI.Title("Player - Options");

            var result = RPGMakerGUI.FoldoutToolBar(ref showDifficulties, "Difficulties", new string[] {"Add Difficulty"});
            if(showDifficulties)
            {
                for (int index = 0; index < Player.Difficulties.Count; index++)
                {
                    var difficulty = Player.Difficulties[index];
                    GUILayout.BeginHorizontal();
                    difficulty.Name = RPGMakerGUI.TextField("",difficulty.Name);
                    difficulty.DamageMultiplier = EditorGUILayout.Slider("Damage Multiplier:", difficulty.DamageMultiplier, 0.01f, 25.0f);
                    GUILayout.Label((difficulty.DamageMultiplier * 100) + "%",GUILayout.Width(80));
                    GUILayout.Space(5);
                    if (Player.Difficulties.Count > 1)
                    {
                        if (RPGMakerGUI.DeleteButton(25))
                        {
                            Player.Difficulties.Remove(difficulty);
                            index--;
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                if(result == 0)
                {
                    Player.Difficulties.Add(new DifficultyDefinition("New Difficulty",100));
                }

                RPGMakerGUI.EndFoldout();
            }

            #region StartingDifficulty

            RPGMakerGUI.PopupID<DifficultyDefinition>("Starting Difficulty:",ref Player.DefaultDifficultyID);

            #endregion

            Player.ManualAssignAttrPerLevel = RPGMakerGUI.Toggle("Manual Assign Attributes?",
                                                                         Player.ManualAssignAttrPerLevel);
            if (Player.ManualAssignAttrPerLevel)
            {
                Player.AttributePointsEarnedPerLevel = RPGMakerGUI.IntField("Points per level:",
                                                                                Player.AttributePointsEarnedPerLevel);
            }


            Player.LevelUpSound.Audio = RPGMakerGUI.AudioClipSelector("Level Up Sound:", Player.LevelUpSound.Audio,
                                                                ref Player.LevelUpSound.AudioPath);

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        #endregion


        #region Classes
        private static bool mainInfoFoldout = true;
        private static bool animFoldout = true;
        private static bool skillSusFoldout = true;
        private static bool skillImmunFoldout = true;
        private static bool showMecanimAnim = true;
        private static bool wepFoldout = true;
        private static bool[] advWepFoldout = new bool[99];

        private static bool startAttrFoldout = true;
        private static bool startStatFoldout = true;
        private static bool startVitalFoldout = true;
        private static bool startTraitFoldout = true;
        private static bool projectileInfoFoldout = true;

        private static bool startItemsFoldout = true;
        private static bool startEquipFoldout = true;

        private static bool attrPerLevelFoldout = true;
        private static bool startingSkillsFoldout = true;
        private static bool startingTalentsFoldout = true;
        private static int[] selectedStartSkill = new int[999];
        private static int[] selectedSkillSus = new int[999];
        private static int[] selectedSkillImmunity = new int[999];
        private static int[] selectedAnim = new int[999];
        private static int selectedStartScene = 0;
        private static Rm_ClassDefinition selectedClassInfo = null;
        private static GameObject selectedPrefab = null;
        private static List<AnimationDefinition> KeyAnimations
        {
            get{return new List<AnimationDefinition>()
                           {
                               selectedClassInfo.LegacyAnimations.UnarmedAnim,
                               selectedClassInfo.LegacyAnimations.WalkAnim,
                               selectedClassInfo.LegacyAnimations.WalkBackAnim,
                               selectedClassInfo.LegacyAnimations.RunAnim,
                               selectedClassInfo.LegacyAnimations.JumpAnim,
                               selectedClassInfo.LegacyAnimations.TurnRightAnim,
                               selectedClassInfo.LegacyAnimations.TurnLeftAnim,
                               selectedClassInfo.LegacyAnimations.StrafeRightAnim,
                               selectedClassInfo.LegacyAnimations.StrafeLeftAnim,
                               selectedClassInfo.LegacyAnimations.IdleAnim,
                               selectedClassInfo.LegacyAnimations.CombatIdleAnim,
                               selectedClassInfo.LegacyAnimations.TakeHitAnim,
                               selectedClassInfo.LegacyAnimations.FallAnim,
                               selectedClassInfo.LegacyAnimations.DeathAnim,
                               selectedClassInfo.LegacyAnimations.KnockBackAnim,
                               selectedClassInfo.LegacyAnimations.KnockUpAnim
                           };}
        }

        private static GameObject gameObject = null;
        public static void Classes(Rect fullArea, Rect leftArea, Rect mainArea)
        {
            var list = Rm_RPGHandler.Instance.Player.ClassDefinitions;
            GUI.Box(leftArea, "", "backgroundBox");
            GUI.Box(mainArea, "", "backgroundBoxMain");

            GUILayout.BeginArea(PadRect(leftArea, 0, 0));
            RPGMakerGUI.ListArea(list, ref selectedClassInfo, Rm_ListAreaType.Classes, false, true);
            GUILayout.EndArea();


            GUILayout.BeginArea(mainArea);
            RPGMakerGUI.Title("Classes");
            if (selectedClassInfo != null)
            {
                RPGMakerGUI.BeginScrollView();
                if (RPGMakerGUI.Foldout(ref mainInfoFoldout, "Main Info"))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical(GUILayout.MaxWidth(85));
                    selectedClassInfo.Image = RPGMakerGUI.ImageSelector("", selectedClassInfo.Image,
                                                                        ref selectedClassInfo.ImagePath);

                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    selectedClassInfo.Name = RPGMakerGUI.TextField("Name: ", selectedClassInfo.Name);

                    GUILayout.BeginHorizontal();
                    gameObject = RPGMakerGUI.PrefabSelector("Class Prefab:", gameObject, ref selectedClassInfo.ClassPrefabPath);
                    gameObject = RPGMakerGUI.PrefabGeneratorButton(Rmh_PrefabType.Player_Class, gameObject, ref selectedClassInfo.ClassPrefabPath, null, selectedClassInfo.ID);
                    GUILayout.EndHorizontal(); 

                    RPGMakerGUI.PopupID<ExpDefinition>("Exp Definition:", ref selectedClassInfo.ExpDefinitionID);


                    selectedClassInfo.AttackStyle = (AttackStyle) RPGMakerGUI.EnumPopup("Attack Style:", selectedClassInfo.AttackStyle);
                    

                    selectedClassInfo.StartingGold = RPGMakerGUI.IntField("Starting Gold:", selectedClassInfo.StartingGold);
                    
                    //todo: make this better (look nicer)
                    if(RPGMakerGUI.Toggle("Starting at World Location?", ref selectedClassInfo.StartAtWorldLocation))
                    {

                        RPGMakerGUI.PopupID<WorldArea>("Initial World Area:", ref selectedClassInfo.StartingWorldArea);
                        RPGMakerGUI.PopupID<Location>("Initial Locaiton:", ref selectedClassInfo.StartingLocation);
                    }
                    else
                    {
                        RPGMakerGUI.SceneSelector("Starting Scene:", ref selectedClassInfo.StartingScene);
                    }

                    if(RPGMakerGUI.Toggle("Has Starting Quest?",ref selectedClassInfo.HasStartingQuest))
                    {
                        RPGMakerGUI.PopupID<Quest>("Starting Quest:", ref selectedClassInfo.StartingQuestID);
                    }

                    RPGMakerGUI.Label("Unarmed:");
                    selectedClassInfo.UnarmedAttackDamage = RPGMakerGUI.IntField("- Attack Damage:", selectedClassInfo.UnarmedAttackDamage);
                    selectedClassInfo.UnarmedAttackRange = RPGMakerGUI.FloatField("- Attack Range:", selectedClassInfo.UnarmedAttackRange);
                    selectedClassInfo.UnarmedAttackSpeed = RPGMakerGUI.FloatField("- Attack Speed:", selectedClassInfo.UnarmedAttackSpeed);

                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    RPGMakerGUI.EndFoldout();
                }

                #region AutoAttack Details

                if (RPGMakerGUI.Foldout(ref projectileInfoFoldout, "Auto Attack Details"))
                {

                    GUILayout.BeginHorizontal();
                    var isMelee = selectedClassInfo.AttackStyle == AttackStyle.Melee;
                    var autoAttackType = isMelee ? "Melee Effect Prefab:" : "Projectile Prefab:";
                    selectedPrefab = RPGMakerGUI.PrefabSelector(autoAttackType, selectedPrefab,
                                                                        ref selectedClassInfo.AutoAttackPrefabPath);
                    selectedPrefab = RPGMakerGUI.PrefabGeneratorButton(isMelee ? Rmh_PrefabType.Melee_Effect : Rmh_PrefabType.Auto_Attack_Projectile, selectedPrefab, ref selectedClassInfo.AutoAttackPrefabPath);
                    GUILayout.EndHorizontal();


                    if (selectedClassInfo.AttackStyle == AttackStyle.Ranged)
                    {
                        selectedClassInfo.ProjectileSpeed = RPGMakerGUI.FloatField("Projectile Speed:", selectedClassInfo.ProjectileSpeed);

                        selectedClassInfo.ProjectileTravelSound.Audio = RPGMakerGUI.AudioClipSelector("Projectile Travel Sound:", selectedClassInfo.ProjectileTravelSound.Audio, ref selectedClassInfo.ProjectileTravelSound.AudioPath);
                    }


                    GUILayout.BeginHorizontal();
                    selectedPrefab = RPGMakerGUI.PrefabSelector("Impact Prefab:", selectedPrefab,
                                                                        ref selectedClassInfo.AutoAttackImpactPrefabPath);
                    selectedPrefab = RPGMakerGUI.PrefabGeneratorButton(Rmh_PrefabType.Impact, selectedPrefab, ref selectedClassInfo.AutoAttackImpactPrefabPath);
                    GUILayout.EndHorizontal();


                    selectedClassInfo.AutoAttackImpactSound.Audio = RPGMakerGUI.AudioClipSelector("Impact Sound:", selectedClassInfo.AutoAttackImpactSound.Audio, ref selectedClassInfo.AutoAttackImpactSound.AudioPath);


                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region Animation
                var foundAnim = false;
                String[] anims = new string[0];
                if(animFoldout || wepFoldout || advWepFoldout.Any(a => a))
                {
                    var activeObj = Selection.activeObject;
                    var selected = activeObj as GameObject;
                    Animation animation = null;
                    foundAnim = false;
                    if (selected != null)
                    {
                        animation = selected.GetComponent<Animation>();
                        if (animation != null)
                        {
                            if (animation.GetClipCount() > 0)
                                foundAnim = true;
                        }
                    }

                    if (foundAnim)
                    {
                        anims = new string[animation.GetClipCount() + 1];
                        anims[0] = "None";
                        int counter = 1;
                        foreach (AnimationState state in animation)
                        {
                            if (counter > animation.GetClipCount()) break;
                            anims[counter] = state.name;
                            counter++;
                        }
                    }
                }

                    #region Core Anims

                    if (RPGMakerGUI.Foldout(ref animFoldout, "Core Animations"))
                    {
                        GUILayout.BeginHorizontal("backgroundBox");
                        GUILayout.Label("ID", GUILayout.Width(100));
                        GUILayout.Label("Animation", GUILayout.Width(150));
                        GUILayout.Label("Speed", GUILayout.Width(70));
                        GUILayout.Label("Sound", GUILayout.Width(150));
                        GUILayout.Label("Reverse?", GUILayout.Width(100));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                        if (foundAnim)
                        {
                            for (int index = 0; index < KeyAnimations.Count; index++)
                            {
                                var anim = KeyAnimations[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));

                                selectedAnim[index] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                          ? Array.IndexOf(anims, anim.Animation)
                                                          : 0;

                                selectedAnim[index] = EditorGUILayout.Popup(selectedAnim[index], anims,
                                                                            GUILayout.Width(150));
                                anim.Animation = anims[selectedAnim[index]];
                                if (anim.Animation == "None") anim.Animation = "";
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                        }
                        else
                        {
                            EditorGUILayout.HelpBox(
                                "Select a gameObject with the animation component and animations to get drop-downs for animation name instead of typing it in.",
                                MessageType.Info);
                            foreach (var anim in KeyAnimations)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));
                                anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                        }

                        RPGMakerGUI.EndFoldout();
                    }


                    var defaultButtons = new List<string>() {"+Animation"};
                    if (Rm_RPGHandler.Instance.Items.AllowTwoHanded) defaultButtons.Add("+2H Animation");
                    if (Rm_RPGHandler.Instance.Items.EnableOffHandSlot && Rm_RPGHandler.Instance.Items.AllowDualWield) defaultButtons.Add("+DW Animation");
                    var wepAnimResult = RPGMakerGUI.FoldoutToolBar(ref wepFoldout, "Default Attack Animations",
                                                                   defaultButtons.ToArray());
                    if (wepFoldout)
                    {
                        var listOfAnim = selectedClassInfo.LegacyAnimations.DefaultAttackAnimations;
                        var listOf2hAnim = selectedClassInfo.LegacyAnimations.Default2HAttackAnimations;
                        var listOfDWAnim = selectedClassInfo.LegacyAnimations.DefaultDWAttackAnimations;

                        GUILayout.BeginHorizontal("backgroundBox");
                        GUILayout.Label("ID", GUILayout.Width(100));
                        GUILayout.Label("Animation", GUILayout.Width(150));
                        GUILayout.Label("Speed", GUILayout.Width(70));
                        GUILayout.Label("Impact Time", GUILayout.Width(70));
                        GUILayout.Label("Sound", GUILayout.Width(150));
                        GUILayout.Label("Reverse?", GUILayout.Width(100));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                        if (foundAnim)
                        {
                            for (int index = 0; index < listOfAnim.Count; index++)
                            {
                                var anim = listOfAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));

                                selectedAnim[index] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                          ? Array.IndexOf(anims, anim.Animation)
                                                          : 0;

                                selectedAnim[index] = EditorGUILayout.Popup(selectedAnim[index], anims,
                                                                            GUILayout.Width(120));
                                anim.Animation = anims[selectedAnim[index]];
                                if (anim.Animation == "None") anim.Animation = "";
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOfAnim.Remove(anim);
                                    index--;
                                }
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }

                            for (int index = 0; index < listOf2hAnim.Count; index++)
                            {
                                var anim = listOf2hAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));

                                selectedAnim[index] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                          ? Array.IndexOf(anims, anim.Animation)
                                                          : 0;

                                selectedAnim[index] = EditorGUILayout.Popup(selectedAnim[index], anims,
                                                                            GUILayout.Width(120));
                                anim.Animation = anims[selectedAnim[index]];
                                if (anim.Animation == "None") anim.Animation = "";
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOf2hAnim.Remove(anim);
                                    index--;
                                }
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }

                            for (int index = 0; index < listOfDWAnim.Count; index++)
                            {
                                var anim = listOfDWAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));

                                selectedAnim[index] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                          ? Array.IndexOf(anims, anim.Animation)
                                                          : 0;

                                selectedAnim[index] = EditorGUILayout.Popup(selectedAnim[index], anims,
                                                                            GUILayout.Width(120));
                                anim.Animation = anims[selectedAnim[index]];
                                if (anim.Animation == "None") anim.Animation = "";
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOfDWAnim.Remove(anim);
                                    index--;
                                }
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                        }
                        else
                        {
                            EditorGUILayout.HelpBox(
                                "Select a gameObject with the animation component and animations to get drop-downs for animation name instead of typing it in.",
                                MessageType.Info);
                            for (int index = 0; index < listOfAnim.Count; index++)
                            {
                                var anim = listOfAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));
                                anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOfAnim.Remove(anim);
                                    index--;
                                }
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                            for (int index = 0; index < listOf2hAnim.Count; index++)
                            {
                                var anim = listOf2hAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));
                                anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOf2hAnim.Remove(anim);
                                    index--;
                                } GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                            for (int index = 0; index < listOfDWAnim.Count; index++)
                            {
                                var anim = listOfDWAnim[index];
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(anim.Name, GUILayout.Width(100));
                                anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                           GUILayout.Width(150));
                                anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                if (RPGMakerGUI.DeleteButton(15))
                                {
                                    listOfDWAnim.Remove(anim);
                                    index--;
                                } GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                            }
                        }

                        if (wepAnimResult == 0)
                        {
                            var count = 0;
                            while (listOfAnim.FirstOrDefault(l => l.Name == "Attack " + count) != null)
                            {
                                count++;
                            }
                            selectedClassInfo.LegacyAnimations.DefaultAttackAnimations.Add(new AnimationDefinition()
                                                                                               {
                                                                                                   Name = "Attack " + count
                                                                                               });
                        }
                        else if (wepAnimResult == 1)
                        {
                            var count = 0;
                            while (listOf2hAnim.FirstOrDefault(l => l.Name == "2H Attack " + count) != null)
                            {
                                count++;
                            }
                            selectedClassInfo.LegacyAnimations.Default2HAttackAnimations.Add(new AnimationDefinition()
                                                                                                 {
                                                                                                     Name =
                                                                                                         "2H Attack " +
                                                                                                         count
                                                                                                 });
                        }
                        else if (wepAnimResult == 2)
                        {
                            var count = 0;
                            while (listOfDWAnim.FirstOrDefault(l => l.Name == "DW Attack " + count) != null)
                            {
                                count++;
                            }
                            selectedClassInfo.LegacyAnimations.DefaultDWAttackAnimations.Add(new AnimationDefinition()
                                                                                                 {
                                                                                                     Name =
                                                                                                         "DW Attack " +
                                                                                                         count
                                                                                                 });
                        }
                        RPGMakerGUI.EndFoldout();
                    }

                    #endregion


                    var wepAnimList = selectedClassInfo.LegacyAnimations.WeaponAnimations;

                    #region Check For Updates
                    foreach (var d in Rm_RPGHandler.Instance.Items.WeaponTypes)
                    {
                        var wepType = wepAnimList.FirstOrDefault(t => t.WeaponTypeID == d.ID);
                        if (wepType == null)
                        {
                            var wepTypeToAdd = new WeaponAnimationDefinition() { WeaponTypeID = d.ID };
                            wepAnimList.Add(wepTypeToAdd);
                        }
                    }

                    for (int index = 0; index < wepAnimList.Count; index++)
                    {
                        var v = wepAnimList[index];
                        var stillExists =
                            Rm_RPGHandler.Instance.Items.WeaponTypes.FirstOrDefault(t => t.ID == v.WeaponTypeID);

                        if (stillExists == null)
                        {
                            wepAnimList.Remove(v);
                            index--;
                        }
                    }
                    #endregion
                    for (int index = 0; index < wepAnimList.Count; index++)
                    {
                        wepAnimList = selectedClassInfo.LegacyAnimations.WeaponAnimations;
                        var weaponAnimationDefinition = wepAnimList[index];
                        var wepTypeDef =
                            Rm_RPGHandler.Instance.Items.WeaponTypes.First(
                                w => w.ID == weaponAnimationDefinition.WeaponTypeID);
                        var wepType = wepTypeDef.Name;

                        var buttons = new List<string>(){"+Animation"};
                        if(wepTypeDef.AllowDualWield && Rm_RPGHandler.Instance.Items.AllowDualWield) buttons.Add("+DW Animation");

                        var wepTypeResult = RPGMakerGUI.FoldoutToolBar(ref advWepFoldout[index],
                                                                       "Custom " + wepType +
                                                                       " Animations", buttons.ToArray());
                        if (advWepFoldout[index])
                        {
                            if(weaponAnimationDefinition.Animations.Count == 0 && weaponAnimationDefinition.DualWieldAnimations.Count == 0)
                            {
                                EditorGUILayout.HelpBox("Click one of the +Animation buttons to add custom animations for this weapon type.", MessageType.Info);
                            }

                            var listOfWepAnim = weaponAnimationDefinition.Animations;
                            var listOfWepAnimDW = weaponAnimationDefinition.DualWieldAnimations;

                            GUILayout.BeginHorizontal("backgroundBox");
                            GUILayout.Label("ID", GUILayout.Width(100));
                            GUILayout.Label("Animation", GUILayout.Width(150));
                            GUILayout.Label("Speed", GUILayout.Width(70));
                            GUILayout.Label("Impact Time", GUILayout.Width(70));
                            GUILayout.Label("Sound", GUILayout.Width(150));
                            GUILayout.Label("Reverse?", GUILayout.Width(100));
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            if (foundAnim)
                            {
                                for (int Windex = 0; Windex < listOfWepAnim.Count; Windex++)
                                {
                                    var anim = listOfWepAnim[Windex];
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(anim.Name, GUILayout.Width(100));

                                    selectedAnim[Windex] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                               ? Array.IndexOf(anims, anim.Animation)
                                                               : 0;

                                    selectedAnim[Windex] = EditorGUILayout.Popup(selectedAnim[Windex], anims,
                                                                                 GUILayout.Width(120));
                                    anim.Animation = anims[selectedAnim[Windex]];
                                    if (anim.Animation == "None") anim.Animation = "";
                                    anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                    anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                    anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                               GUILayout.Width(150));
                                    anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                    if(RPGMakerGUI.DeleteButton(15))
                                    {
                                        listOfWepAnim.Remove(anim);
                                        index--;
                                    }
                                    GUILayout.FlexibleSpace();
                                    GUILayout.EndHorizontal();
                                }

                                for (int Dindex = 0; Dindex < listOfWepAnimDW.Count; Dindex++)
                                {
                                    var anim = listOfWepAnimDW[Dindex];
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(anim.Name, GUILayout.Width(100));

                                    selectedAnim[Dindex] = anims.FirstOrDefault(s => s == anim.Animation) != null
                                                               ? Array.IndexOf(anims, anim.Animation)
                                                               : 0;

                                    selectedAnim[Dindex] = EditorGUILayout.Popup(selectedAnim[Dindex], anims,
                                                                                 GUILayout.Width(120));
                                    anim.Animation = anims[selectedAnim[Dindex]];
                                    if (anim.Animation == "None") anim.Animation = "";
                                    anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                    anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                    anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                               GUILayout.Width(150));
                                    anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                    if (RPGMakerGUI.DeleteButton(15))
                                    {
                                        listOfWepAnimDW.Remove(anim);
                                        Dindex--;
                                    } 
                                    GUILayout.FlexibleSpace();
                                    GUILayout.EndHorizontal();
                                }
                            }
                            else
                            {
                                for (int i = 0; i < listOfWepAnim.Count; i++)
                                {
                                    var anim = listOfWepAnim[i];
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(anim.Name, GUILayout.Width(100));
                                    anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                    anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                    anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                    anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                               GUILayout.Width(150));
                                    anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                    if (RPGMakerGUI.DeleteButton(15))
                                    {
                                        listOfWepAnim.Remove(anim);
                                        i--;
                                    }
                                    GUILayout.FlexibleSpace();
                                    GUILayout.EndHorizontal();
                                }
                                for (int i = 0; i < listOfWepAnimDW.Count; i++)
                                {
                                    var anim = listOfWepAnimDW[i];
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(anim.Name, GUILayout.Width(100));
                                    anim.Animation = RPGMakerGUI.TextField("", anim.Animation, GUILayout.Width(150));
                                    anim.Speed = RPGMakerGUI.FloatField("", anim.Speed, GUILayout.Width(70));
                                    anim.ImpactTime = RPGMakerGUI.FloatField("", anim.ImpactTime, GUILayout.Width(70));
                                    anim.Sound = RPGMakerGUI.AudioClipSelector("", anim.Sound, ref anim.SoundPath,
                                                                               GUILayout.Width(150));
                                    anim.Backwards = RPGMakerGUI.Toggle(anim.Backwards);
                                    if (RPGMakerGUI.DeleteButton(15))
                                    {
                                        listOfWepAnimDW.Remove(anim);
                                        i--;
                                    }
                                    GUILayout.FlexibleSpace();
                                    GUILayout.EndHorizontal();
                                }
                            }


                            if (wepTypeResult == 0)
                            {
                                var count = 0;
                                while (listOfWepAnim.FirstOrDefault(l => l.Name == "Attack " + count) != null)
                                {
                                    count++;
                                }
                                listOfWepAnim.Add(new AnimationDefinition() { Name = "Attack " + count });
                            }
                            else if (wepTypeResult == 1)
                            {
                                var count = 0;
                                while (listOfWepAnimDW.FirstOrDefault(l => l.Name == "DW Attack " + count) != null)
                                {
                                    count++;
                                }
                                listOfWepAnimDW.Add(new AnimationDefinition() { Name = "DW Attack " + count });
                            }

                            RPGMakerGUI.EndFoldout();
                        }
                    }
                    #endregion


                #region Starting Attributes

                
                if (RPGMakerGUI.Foldout(ref startAttrFoldout, "Starting Attributes"))
                {
                    if (Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.Count > 0)
                    {
                        foreach (var d in Rm_RPGHandler.Instance.ASVT.AttributesDefinitions)
                        {
                            var attr = selectedClassInfo.StartingAttributes.FirstOrDefault(a => a.AsvtID == d.ID);
                            if (attr == null)
                            {
                                selectedClassInfo.StartingAttributes.Add(new Rm_AsvtAmount() {AsvtID = d.ID,Amount = d.DefaultValue});
                            }
                        }

                        for (int index = 0; index < selectedClassInfo.StartingAttributes.Count; index++)
                        {
                            var v = selectedClassInfo.StartingAttributes[index];
                            var stillExists =
                                Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.FirstOrDefault(
                                    a => a.ID == v.AsvtID);

                            if (stillExists == null)
                            {
                                selectedClassInfo.StartingAttributes.Remove(v);
                                index--;
                            }
                        }

                        foreach (var v in selectedClassInfo.StartingAttributes)
                        {
                            var prefix =
                                Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.First(a => a.ID == v.AsvtID).Name;
                            v.Amount = RPGMakerGUI.IntField(prefix, v.Amount);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No defined attributes.", MessageType.Info);
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region Starting Statistics

                if (RPGMakerGUI.Foldout(ref startStatFoldout, "Starting Statistics"))
                {
                    if (Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.Count > 0)
                    {
                        foreach (var d in Rm_RPGHandler.Instance.ASVT.StatisticDefinitions)
                        {
                            var attr = selectedClassInfo.StartingStats.FirstOrDefault(a => a.AsvtID == d.ID);
                            if (attr == null)
                            {
                                selectedClassInfo.StartingStats.Add(new Rm_AsvtAmountFloat() { AsvtID = d.ID, Amount = d.DefaultValue });
                            }
                        }

                        for (int index = 0; index < selectedClassInfo.StartingStats.Count; index++)
                        {
                            var v = selectedClassInfo.StartingStats[index];
                            var stillExists =
                                Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.FirstOrDefault(
                                    a => a.ID == v.AsvtID);

                            if (stillExists == null)
                            {
                                selectedClassInfo.StartingStats.Remove(v);
                                index--;
                            }
                        }

                        foreach (var v in selectedClassInfo.StartingStats)
                        {
                            var prefix =
                                Rm_RPGHandler.Instance.ASVT.StatisticDefinitions.First(a => a.ID == v.AsvtID).Name;
                            v.Amount = RPGMakerGUI.FloatField(prefix, v.Amount);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No defined statistics.", MessageType.Info);
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region Starting Vitals

                if (RPGMakerGUI.Foldout(ref startVitalFoldout, "Starting Vitals"))
                {
                    if (Rm_RPGHandler.Instance.ASVT.VitalDefinitions.Count > 0)
                    {
                        foreach (var d in Rm_RPGHandler.Instance.ASVT.VitalDefinitions)
                        {
                            var attr = selectedClassInfo.StartingVitals.FirstOrDefault(a => a.AsvtID == d.ID);
                            if (attr == null)
                            {
                                selectedClassInfo.StartingVitals.Add(new Rm_AsvtAmount() { AsvtID = d.ID, Amount = d.DefaultValue });
                            }
                        }

                        for (int index = 0; index < selectedClassInfo.StartingVitals.Count; index++)
                        {
                            var v = selectedClassInfo.StartingVitals[index];
                            var stillExists =
                                Rm_RPGHandler.Instance.ASVT.VitalDefinitions.FirstOrDefault(
                                    a => a.ID == v.AsvtID);

                            if (stillExists == null)
                            {
                                selectedClassInfo.StartingVitals.Remove(v);
                                index--;
                            }
                        }

                        foreach (var v in selectedClassInfo.StartingVitals)
                        {
                            var prefix =
                                Rm_RPGHandler.Instance.ASVT.VitalDefinitions.First(a => a.ID == v.AsvtID).Name;
                            v.Amount = RPGMakerGUI.IntField(prefix, v.Amount);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No defined vitals.", MessageType.Info);
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region Starting Traits

                if (RPGMakerGUI.Foldout(ref startTraitFoldout, "Starting Traits"))
                {
                    if (Rm_RPGHandler.Instance.ASVT.TraitDefinitions.Count > 0)
                    {
                        foreach (var d in Rm_RPGHandler.Instance.ASVT.TraitDefinitions)
                        {
                            var attr = selectedClassInfo.StartingTraitLevels.FirstOrDefault(a => a.AsvtID == d.ID);
                            if (attr == null)
                            {
                                selectedClassInfo.StartingTraitLevels.Add(new Rm_AsvtAmount() { AsvtID = d.ID, Amount = d.StartingLevel });
                            }
                        }

                        for (int index = 0; index < selectedClassInfo.StartingTraitLevels.Count; index++)
                        {
                            var v = selectedClassInfo.StartingTraitLevels[index];
                            var stillExists =
                                Rm_RPGHandler.Instance.ASVT.TraitDefinitions.FirstOrDefault(
                                    a => a.ID == v.AsvtID);

                            if (stillExists == null)
                            {
                                selectedClassInfo.StartingTraitLevels.Remove(v);
                                index--;
                            }
                        }

                        foreach (var v in selectedClassInfo.StartingTraitLevels)
                        {
                            var prefix =
                                Rm_RPGHandler.Instance.ASVT.TraitDefinitions.First(a => a.ID == v.AsvtID).Name;
                            v.Amount = RPGMakerGUI.IntField(prefix, v.Amount);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No defined traits.", MessageType.Info);
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                if (!Rm_RPGHandler.Instance.Player.ManualAssignAttrPerLevel)
                {
                    #region Attributes Per Level
                    if (RPGMakerGUI.Foldout(ref attrPerLevelFoldout, "Attributes Per Level"))
                    {
                        if (Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.Count > 0)
                        {
                            foreach (var d in Rm_RPGHandler.Instance.ASVT.AttributesDefinitions)
                            {
                                var attr =
                                    selectedClassInfo.AttributePerLevel.FirstOrDefault(a => a.AsvtID == d.ID);
                                if (attr == null)
                                {
                                    selectedClassInfo.AttributePerLevel.Add(new Rm_AsvtAmount() {AsvtID = d.ID});
                                }
                            }

                            for (int index = 0; index < selectedClassInfo.AttributePerLevel.Count; index++)
                            {
                                var v = selectedClassInfo.AttributePerLevel[index];
                                var stillExists =
                                    Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.FirstOrDefault(
                                        a => a.ID == v.AsvtID);

                                if (stillExists == null)
                                {
                                    selectedClassInfo.AttributePerLevel.Remove(v);
                                    index--;
                                }
                            }

                            foreach (var v in selectedClassInfo.AttributePerLevel)
                            {
                                var prefix =
                                    Rm_RPGHandler.Instance.ASVT.AttributesDefinitions.First(a => a.ID == v.AsvtID).
                                        Name;
                                v.Amount = RPGMakerGUI.IntField(prefix, v.Amount);
                            }
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("No defined attributes.", MessageType.Info);
                        }

                    }
                    if (attrPerLevelFoldout) RPGMakerGUI.EndFoldout();

                    #endregion
                }


                #region SkillImmunities
                var immunityResult = RPGMakerGUI.FoldoutToolBar(ref skillImmunFoldout, "Skill Immunities", new string[] { "+Immunity" });
                if (skillImmunFoldout)
                {
                    for (int index = 0; index < selectedClassInfo.SkillMetaImmunitiesID.Count; index++)
                    {
                        var v = selectedClassInfo.SkillMetaImmunitiesID[index];
                        if (string.IsNullOrEmpty(v)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Combat.SkillMeta.FirstOrDefault(
                                a => a.ID == v);

                        if (stillExists == null)
                        {
                            selectedClassInfo.SkillMetaImmunitiesID.Remove(v);
                            index--;
                        }
                    }
                    var allSkillMetas = Rm_RPGHandler.Instance.Combat.SkillMeta;
                    if (allSkillMetas.Count > 0)
                    {
                        if (selectedClassInfo.SkillMetaImmunitiesID.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Click +Immunity to add a skill meta this class is immune to.", MessageType.Info);
                        }

                        GUILayout.Space(5);
                        for (int index = 0; index < selectedClassInfo.SkillMetaImmunitiesID.Count; index++)
                        {
                            GUILayout.BeginHorizontal();
                            if (string.IsNullOrEmpty(selectedClassInfo.SkillMetaImmunitiesID[index]))
                            {
                                selectedSkillImmunity[index] = 0;
                            }
                            else
                            {
                                var stillExists =
                                    allSkillMetas.FirstOrDefault(a => a.ID == selectedClassInfo.SkillMetaImmunitiesID[index]);
                                selectedSkillImmunity[index] = stillExists != null ? allSkillMetas.IndexOf(stillExists) : 0;
                            }

                            selectedSkillImmunity[index] = EditorGUILayout.Popup("Immunity:", selectedSkillImmunity[index],
                                                                            allSkillMetas.Select((q, indexOf) => indexOf + ". " + q.Name).
                                                                                ToArray());
                            selectedClassInfo.SkillMetaImmunitiesID[index] = allSkillMetas[selectedSkillImmunity[index]].ID;

                            if (GUILayout.Button(RPGMakerGUI.DelIcon, "genericButton", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                selectedClassInfo.SkillMetaImmunitiesID.Remove(selectedClassInfo.SkillMetaImmunitiesID[index]);
                                index--;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Skill Metas Found.", MessageType.Info);
                        selectedClassInfo.SkillMetaImmunitiesID = new List<string>();
                    }

                    if (immunityResult == 0)
                    {
                        selectedClassInfo.SkillMetaImmunitiesID.Add("");
                    }
                }
                if (skillImmunFoldout) RPGMakerGUI.EndFoldout();
                #endregion

                #region SkillSusceptibility
                var susceptResult = RPGMakerGUI.FoldoutToolBar(ref skillSusFoldout, "Skill Susceptibilities", new string[] { "+Susceptibility" });
                if (skillSusFoldout)
                {
                    for (int index = 0; index < selectedClassInfo.SkillMetaSusceptibilities.Count; index++)
                    {
                        var v = selectedClassInfo.SkillMetaSusceptibilities[index];
                        if (string.IsNullOrEmpty(v.ID)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Combat.SkillMeta.FirstOrDefault(
                                a => a.ID == v.ID);

                        if (stillExists == null)
                        {
                            selectedClassInfo.SkillMetaSusceptibilities.Remove(v);
                            index--;
                        }
                    }
                    var allSkillMetas = Rm_RPGHandler.Instance.Combat.SkillMeta;
                    if (allSkillMetas.Count > 0)
                    {
                        if (selectedClassInfo.SkillMetaSusceptibilities.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Click +Susceptibility to add a skill meta this class takes more damage from.", MessageType.Info);
                        }

                        GUILayout.Space(5);
                        for (int index = 0; index < selectedClassInfo.SkillMetaSusceptibilities.Count; index++)
                        {
                            GUILayout.BeginHorizontal();
                            if (string.IsNullOrEmpty(selectedClassInfo.SkillMetaSusceptibilities[index].ID))
                            {
                                selectedSkillSus[index] = 0;
                            }
                            else
                            {
                                var stillExists =
                                    allSkillMetas.FirstOrDefault(a => a.ID == selectedClassInfo.SkillMetaSusceptibilities[index].ID);
                                selectedSkillSus[index] = stillExists != null ? allSkillMetas.IndexOf(stillExists) : 0;
                            }
                            GUILayout.BeginHorizontal();
                            selectedSkillSus[index] = EditorGUILayout.Popup("Susceptibility:", selectedSkillSus[index],
                                                                            allSkillMetas.Select((q, indexOf) => indexOf + ". " + q.Name).
                                                                                ToArray());
                            var selectedSus = selectedClassInfo.SkillMetaSusceptibilities[index];
                            selectedSus.AdditionalDamage = RPGMakerGUI.FloatField("Additional Damage:", selectedSus.AdditionalDamage);
                            GUILayout.EndHorizontal();
                            selectedClassInfo.SkillMetaSusceptibilities[index].ID = allSkillMetas[selectedSkillSus[index]].ID;

                            if (GUILayout.Button(RPGMakerGUI.DelIcon, "genericButton", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                selectedClassInfo.SkillMetaSusceptibilities.Remove(selectedClassInfo.SkillMetaSusceptibilities[index]);
                                index--;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Skill Metas Found.", MessageType.Info);
                        selectedClassInfo.SkillMetaSusceptibilities = new List<SkillMetaSusceptibility>();
                    }

                    if (susceptResult == 0)
                    {
                        selectedClassInfo.SkillMetaSusceptibilities.Add(new SkillMetaSusceptibility());
                    }
                }
                if (skillSusFoldout) RPGMakerGUI.EndFoldout();
                #endregion


                #region StartingSkills

                var result = RPGMakerGUI.FoldoutToolBar(ref startingSkillsFoldout, "Starting Skills", new string[] {"+Skill"});
                if (startingSkillsFoldout)
                {
                    for (int index = 0; index < selectedClassInfo.StartingSkillIds.Count; index++)
                    {
                        var v = selectedClassInfo.StartingSkillIds[index];
                        if (String.IsNullOrEmpty(v)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Repositories.Skills.AllSkills.FirstOrDefault(
                                a => a.ID == v);

                        if (stillExists == null)
                        {
                            selectedClassInfo.StartingSkillIds.Remove(v);
                            index--;
                        }
                    }
                    var allSkills = Rm_RPGHandler.Instance.Repositories.Skills.AllSkills;
                    if (allSkills.Count > 0)
                    {
                        if (selectedClassInfo.StartingSkillIds.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Click +Skill to add a skill this class starts with.", MessageType.Info);
                        }

                        GUILayout.Space(5);
                        for (int index = 0; index < selectedClassInfo.StartingSkillIds.Count; index++)
                        {

                            GUILayout.BeginHorizontal();
                            var skillId = selectedClassInfo.StartingSkillIds[index];
                            RPGMakerGUI.PopupID<Skill>("Skill Name:", ref skillId);
                            selectedClassInfo.StartingSkillIds[index] = skillId;

                            if (GUILayout.Button(RPGMakerGUI.DelIcon, "genericButton", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                selectedClassInfo.StartingSkillIds.Remove(selectedClassInfo.StartingSkillIds[index]);
                                index--;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Skills Found.", MessageType.Warning);
                        selectedClassInfo.StartingSkillIds = new List<string>();
                    }

                    if (result == 0)
                    {
                        selectedClassInfo.StartingSkillIds.Add("");
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region StartingTalents

                var talentResult = RPGMakerGUI.FoldoutToolBar(ref startingTalentsFoldout, "Starting Talents", new string[] {"+Talent"});
                if (startingTalentsFoldout)
                {
                    for (int index = 0; index < selectedClassInfo.StartingTalentIds.Count; index++)
                    {
                        var v = selectedClassInfo.StartingTalentIds[index];
                        if (String.IsNullOrEmpty(v)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Repositories.Talents.AllTalents.FirstOrDefault(
                                a => a.ID == v);

                        if (stillExists == null)
                        {
                            selectedClassInfo.StartingTalentIds.Remove(v);
                            index--;
                        }
                    }
                    var allTalents = Rm_RPGHandler.Instance.Repositories.Talents.AllTalents;
                    if (allTalents.Count > 0)
                    {
                        if (selectedClassInfo.StartingTalentIds.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Click +Talent to add a talent this class starts with.", MessageType.Info);
                        }

                        GUILayout.Space(5);
                        for (int index = 0; index < selectedClassInfo.StartingTalentIds.Count; index++)
                        {

                            GUILayout.BeginHorizontal();
                            var talentId = selectedClassInfo.StartingTalentIds[index];
                            RPGMakerGUI.PopupID<Talent>("Talent Name:", ref talentId);
                            selectedClassInfo.StartingTalentIds[index] = talentId;

                            if (GUILayout.Button(RPGMakerGUI.DelIcon, "genericButton", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                selectedClassInfo.StartingTalentIds.Remove(selectedClassInfo.StartingTalentIds[index]);
                                index--;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Talents Found.", MessageType.Warning);
                        selectedClassInfo.StartingTalentIds = new List<string>();
                    }

                    if (talentResult == 0)
                    {
                        selectedClassInfo.StartingTalentIds.Add("");
                    }
                    RPGMakerGUI.EndFoldout();
                }

                #endregion

                #region "Starting Items"
                var gurResult = RPGMakerGUI.FoldoutToolBar(ref startItemsFoldout, "Starting Items","+Item");
                if (startItemsFoldout)
                {
                    for (int index = 0; index < selectedClassInfo.StartingItems.Count; index++)
                    {
                        var v = selectedClassInfo.StartingItems[index];
                        if (string.IsNullOrEmpty(v.ItemID)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Repositories.Items.AllItems.FirstOrDefault(
                                a => a.ID == v.ItemID);

                        if (stillExists == null)
                        {
                            selectedClassInfo.StartingItems.Remove(v);
                            index--;
                        }
                    }

                    var allItems = Rm_RPGHandler.Instance.Repositories.Items.AllItems;
                    if (allItems.Count > 0)
                    {
                        if (selectedClassInfo.StartingItems.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Click +Item to add an item the player will start with in their inventory.", MessageType.Info);
                        }

                        GUILayout.Space(5);



                        for (int index = 0; index < selectedClassInfo.StartingItems.Count; index++)
                        {
                            GUILayout.BeginHorizontal();
                            var selectedSus = selectedClassInfo.StartingItems[index];
                            RPGMakerGUI.PopupID<Item>("Item:", ref selectedSus.ItemID);

                            var stackable = allItems.FirstOrDefault(i => i.ID == selectedSus.ItemID) as IStackable;
                            if (stackable != null)
                            {
                                selectedSus.Amount = RPGMakerGUI.IntField("Amount:", selectedSus.Amount);
                            }
                            if (GUILayout.Button(RPGMakerGUI.DelIcon, "genericButton", GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                selectedClassInfo.StartingItems.Remove(selectedClassInfo.StartingItems[index]);
                                index--;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No Items Found.", MessageType.Info);
                        selectedClassInfo.StartingItems = new List<LootDefinition>();
                    }

                    if (gurResult == 0)
                    {
                        selectedClassInfo.StartingItems.Add(new LootDefinition());
                    }

                    RPGMakerGUI.EndFoldout();
                }
                #endregion

                #region "Starting Equipped"

                if (RPGMakerGUI.Foldout(ref startEquipFoldout, "Starting Equipped Items"))
                {
                    for (int index = 0; index < selectedClassInfo.StartingEquipped.Count; index++)
                    {
                        var v = selectedClassInfo.StartingEquipped[index];
                        if (string.IsNullOrEmpty(v.SlotID)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Items.ApparelSlots.FirstOrDefault(
                                a => a.ID == v.SlotID);

                        if (stillExists == null)
                        {
                            selectedClassInfo.StartingEquipped.Remove(v);
                            index--;
                        }
                    }

                    for (int index = 0; index < selectedClassInfo.StartingEquipped.Count; index++)
                    {
                        var v = selectedClassInfo.StartingEquipped[index];
                        if (string.IsNullOrEmpty(v.ItemID)) continue;

                        var stillExists =
                            Rm_RPGHandler.Instance.Repositories.Items.AllItems.FirstOrDefault(
                                a => a.ID == v.ItemID);

                        if (stillExists == null)
                        {
                            v.Enabled = false;
                            v.ItemID = "";
                            index--;
                        }
                    }

                    for (var i = 0; i < Rm_RPGHandler.Instance.Items.ApparelSlots.Count; i++)
                    {
                        var slotName = Rm_RPGHandler.Instance.Items.ApparelSlots[i];

                        var exists = selectedClassInfo.StartingEquipped.FirstOrDefault(v => v.SlotID == slotName.ID);
                        if(exists == null)
                        {
                            var newStartEquip = new StartEquipDefinition() {SlotID = slotName.ID, SlotName = slotName.Name, ItemID = ""};
                            selectedClassInfo.StartingEquipped.Add(newStartEquip);    
                        }
                    }


                    //Weapon
                    GUILayout.BeginHorizontal();
                    if (RPGMakerGUI.Toggle("Weapon?", ref selectedClassInfo.StartingEquippedWeapon.Enabled))
                    {
                        RPGMakerGUI.PopupID<Item>("Item:", ref selectedClassInfo.StartingEquippedWeapon.ItemID);
                    }
                    GUILayout.EndHorizontal();

                    //Rest
                    for (var i = 0; i < selectedClassInfo.StartingEquipped.Count; i++)
                    {
                        var slotName = selectedClassInfo.StartingEquipped[i];
                        GUILayout.BeginHorizontal();
                        if (RPGMakerGUI.Toggle(slotName.SlotName + "?", ref slotName.Enabled))
                        {
                            RPGMakerGUI.PopupID<Item>("Item:", ref slotName.ItemID);
                        }
                        GUILayout.EndHorizontal();
                    }

                    RPGMakerGUI.EndFoldout();
                }
                #endregion

                RPGMakerGUI.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("Add or select a new field to customise player classes.", MessageType.Info);
            }
            GUILayout.EndArea();
        }

        #endregion

        public static Rmh_Experience Exp
        {
            get { return Rm_RPGHandler.Instance.Experience; }
        }

        static Rme_Main_Player()
        {
            customExpDefFoldoutBools = new bool[999][];
            
            for (int i = 0; i < Exp.CustomExpDefinitions.Count; i++)
            {
                customExpDefFoldoutBools[i] = new []{false, false, false};
            }
        }

        public static bool showCustomExpDefinitions = true;
        public static Vector2 expScrollPos = Vector2.zero;
        public static bool[] playerExpDefFoldoutBools = new bool[] { true, false, false };
        public static bool[] traitExpDefFoldoutBools = new bool[] { true, false, false };
        public static bool[] monsterExpDefFoldoutBools = new bool[] { true, false, false };
        public static bool[][] customExpDefFoldoutBools;
        public static bool[] titleFoldoutBools = new bool[250];
        public static void Experience(Rect fullArea, Rect leftArea, Rect mainArea)
        {
            GUI.Box(fullArea, "","backgroundBox");

            GUILayout.BeginArea(fullArea);
            expScrollPos = GUILayout.BeginScrollView(expScrollPos);

            ExpFoldout(Exp.PlayerExpDefinition, ref playerExpDefFoldoutBools,0);
            ExpFoldout(Exp.TraitExpDefinition, ref traitExpDefFoldoutBools,1);
            ExpFoldout(Exp.MonsterExpDefinition, ref monsterExpDefFoldoutBools,2);

            var result = RPGMakerGUI.FoldoutToolBar(ref showCustomExpDefinitions, "Custom Exp Definitions", "+ExpDefinition");
            if(showCustomExpDefinitions)
            {
                if (result == 0)
                {
                    Exp.CustomExpDefinitions.Add(new ExpDefinition());
                    customExpDefFoldoutBools[Exp.CustomExpDefinitions.Count - 1] = new []{false, false, false};
                }

                for (int i = 0; i < Exp.CustomExpDefinitions.Count; i++)
                {
                    var expDef = Exp.CustomExpDefinitions[i];
                    ExpFoldout(expDef, ref customExpDefFoldoutBools[i],3 + i, true);
                }

                if (Exp.CustomExpDefinitions.Count == 0)
                {
                    EditorGUILayout.HelpBox(
                        "Click +ExpDefinition to add a new exp definition which classes, traits and monster types can use.",
                        MessageType.Info);
                }

                RPGMakerGUI.EndFoldout();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public static void ExpFoldout(ExpDefinition expDefinition, ref bool[] foldoutBooleans, int titleFoldOutIndex, bool allowDelete = false)
        {
            if(RPGMakerGUI.Foldout(ref titleFoldoutBools[titleFoldOutIndex], "Exp Definition: " + expDefinition.Name))
            {
                if (RPGMakerGUI.Foldout(ref foldoutBooleans[0], "Experience Details"))
                {
                    if (!expDefinition.IsDefault)
                    {
                        expDefinition.Name = RPGMakerGUI.TextField("Name:", expDefinition.Name);
                        expDefinition.ExpUse = (ExpUse)RPGMakerGUI.EnumPopup("Exp Use:", expDefinition.ExpUse);

                    }

                    expDefinition.ExpType = (ExpType)RPGMakerGUI.EnumPopup("Exp Type:", expDefinition.ExpType);
                    if (expDefinition.IsDefault || expDefinition.ExpUse == ExpUse.TraitLevelling)
                    {
                        expDefinition.MaxLevel = EditorGUILayout.IntField("Max Level", expDefinition.MaxLevel);
                    }
                    else
                    {
                        if (expDefinition.ExpUse == ExpUse.PlayerLevelling)
                        {
                            expDefinition.MaxLevel = Exp.AllExpDefinitions.First(e => e.ID == Rmh_Experience.PlayerExpDefinitionID).MaxLevel;
                        }
                        else
                        {
                            expDefinition.MaxLevel = Exp.AllExpDefinitions.First(e => e.ID == Rmh_Experience.MonsterExpDefinitionID).MaxLevel;
                        }
                    }

                    var expInfoText = expDefinition.ExpUse != ExpUse.ExpGained ? "Exp To Level for " : "Exp Gained at ";

                    if (expDefinition.ExpType != ExpType.Linear)
                    {
                        expDefinition.XpForFirstLevel = EditorGUILayout.IntField(expInfoText + "First Level:", expDefinition.XpForFirstLevel);
                    }

                    if (expDefinition.ExpType == ExpType.Logarithmic)
                    {
                        expDefinition.XpForLastLevel = EditorGUILayout.IntField(expInfoText + "Max Level", expDefinition.XpForLastLevel);
                    }

                    if (expDefinition.ExpType != ExpType.Logarithmic)
                    {
                        expDefinition.Constant = EditorGUILayout.IntField("Constant Value:", expDefinition.Constant);
                    }

                    RPGMakerGUI.EndFoldout();
                }

                var expForLevelText = expDefinition.ExpUse != ExpUse.ExpGained ? "Exp To Level " : "Exp Gained At Level ";
                if (RPGMakerGUI.Foldout(ref foldoutBooleans[1], expForLevelText))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    for (int i = 1; i <= expDefinition.MaxLevel; i++)
                    {
                        if (i == (expDefinition.MaxLevel / 3) + 2)
                        {
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                        }
                        if (i == ((expDefinition.MaxLevel / 3) * 2) + 3)
                        {
                            GUILayout.EndVertical();
                            GUILayout.BeginVertical();
                        }

                        GUILayout.Box(i + ":" + expDefinition.ExpForLevel(i), "subTitle", GUILayout.Width(271));
                    }
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    RPGMakerGUI.EndFoldout();
                }

                if (expDefinition.ExpUse != ExpUse.ExpGained)
                {
                    if (RPGMakerGUI.Foldout(ref foldoutBooleans[2], "Max Exp at Level"))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        var total = 0;
                        for (int i = 1; i <= expDefinition.MaxLevel; i++)
                        {
                            if (i == (expDefinition.MaxLevel / 3) + 2)
                            {
                                GUILayout.EndVertical();
                                GUILayout.BeginVertical();
                            }
                            if (i == ((expDefinition.MaxLevel / 3) * 2) + 3)
                            {
                                GUILayout.EndVertical();
                                GUILayout.BeginVertical();
                            }


                            total += expDefinition.ExpForLevel(i);
                            GUILayout.Box(i + ":" + total, "subTitle", GUILayout.Width(271));
                        }
                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        RPGMakerGUI.EndFoldout();
                    }
                }

                if (allowDelete)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Delete", "genericButton", GUILayout.Height(25)))
                    {
                        titleFoldoutBools[titleFoldOutIndex] = false;
                        foldoutBooleans[0] = false;
                        foldoutBooleans[1] = false;
                        foldoutBooleans[2] = false;
                        Exp.CustomExpDefinitions.Remove(expDefinition);
                        return;
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }

                RPGMakerGUI.EndFoldout();
            }
            
        }

        public static Rect PadRect(Rect rect, int left, int top)
        {
            return new Rect(rect.x + left, rect.y + top, rect.width - (left * 2), rect.height - (top * 2));
        }
    }
}
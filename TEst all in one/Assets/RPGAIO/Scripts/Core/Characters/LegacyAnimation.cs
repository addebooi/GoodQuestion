using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicSpawn.RPGMaker.Core
{
    public class LegacyAnimation
    {
        public AnimationDefinition UnarmedAnim;
        public AnimationDefinition WalkAnim;
        public AnimationDefinition WalkBackAnim;
        public AnimationDefinition RunAnim;
        public AnimationDefinition JumpAnim;
        public AnimationDefinition StrafeRightAnim;
        public AnimationDefinition StrafeLeftAnim;
        public AnimationDefinition TurnRightAnim;
        public AnimationDefinition TurnLeftAnim;
        public AnimationDefinition IdleAnim;
        public AnimationDefinition CombatIdleAnim;
        public AnimationDefinition TakeHitAnim;
        public AnimationDefinition FallAnim;
        public AnimationDefinition DeathAnim;
        public AnimationDefinition KnockBackAnim;
        public AnimationDefinition KnockUpAnim;


        public List<AnimationDefinition> DefaultAttackAnimations;
        public List<AnimationDefinition> Default2HAttackAnimations;
        public List<AnimationDefinition> DefaultDWAttackAnimations;
        public List<WeaponAnimationDefinition> WeaponAnimations;

        public LegacyAnimation()
        {
            UnarmedAnim = new AnimationDefinition() { Name = "Unarmed"};
            WalkAnim = new AnimationDefinition() { Name = "Walk"};
            WalkBackAnim = new AnimationDefinition() { Name = "Walk Back"};
            RunAnim = new AnimationDefinition() { Name = "Run"};
            JumpAnim = new AnimationDefinition() { Name = "Jump"};
            TurnRightAnim = new AnimationDefinition() { Name = "Turn Right"};
            TurnLeftAnim = new AnimationDefinition() { Name = "Turn Left" }; 
            StrafeRightAnim = new AnimationDefinition() { Name = "Strafe Right" };
            StrafeLeftAnim = new AnimationDefinition() { Name = "Strafe Left" };
            IdleAnim = new AnimationDefinition() { Name = "Idle"};
            CombatIdleAnim = new AnimationDefinition() { Name = "Combat Idle"};
            TakeHitAnim = new AnimationDefinition() { Name = "Take Hit"};
            FallAnim = new AnimationDefinition() { Name = "Falling"};
            DeathAnim = new AnimationDefinition() { Name = "Death", WrapMode = WrapMode.ClampForever};
            KnockBackAnim = new AnimationDefinition() { Name = "Knock Back"};
            KnockUpAnim = new AnimationDefinition() { Name = "Knock Up"};

            DefaultAttackAnimations = new List<AnimationDefinition>();
            Default2HAttackAnimations = new List<AnimationDefinition>();
            DefaultDWAttackAnimations = new List<AnimationDefinition>();
            WeaponAnimations = new List<WeaponAnimationDefinition>();
        }
    }

    public class WeaponAnimationDefinition
    {
        public string WeaponTypeID;
        public List<AnimationDefinition> Animations;
        public List<AnimationDefinition> DualWieldAnimations;

        public WeaponAnimationDefinition()
        {
            WeaponTypeID = "";
            Animations = new List<AnimationDefinition>();
            DualWieldAnimations = new List<AnimationDefinition>();
        }
    }

    public class AnimationDefinition
    {
        public string Name;
        public string Animation;
        public WrapMode WrapMode;
        public float Speed;
        public bool Backwards;

        public float ImpactTime;

        public AnimationDefinition()
        {
            SoundPath = "";
            Animation = "";
            Speed = 1.0f;
            ImpactTime = 0.7f;
            WrapMode = WrapMode.Loop;
        }
        public string SoundPath;
        [JsonIgnore]
        public AudioClip _sound ;
        [JsonIgnore]
        public AudioClip Sound
        {
            get { return _sound ?? (_sound = Resources.Load(SoundPath) as AudioClip); }
            set { _sound = value; }
        }
    }
}
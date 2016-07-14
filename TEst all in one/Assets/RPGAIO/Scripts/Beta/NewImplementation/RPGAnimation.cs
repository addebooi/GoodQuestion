using System;
using System.Linq;
using Assets.Scripts.Beta.NewImplementation;
using LogicSpawn.RPGMaker;
using LogicSpawn.RPGMaker.Core;
using UnityEngine;

public class RPGAnimation : MonoBehaviour, IRPGAnimation
{
    private IRPGController _controller;
    private string _animationId;

    public Animation Animation
    {
        get { return _controller.Animation; }
    }

    void Awake()
    {
        _controller = GetComponent<RPGController>();
        _animationId = Guid.NewGuid().ToString();
    }

    public void CrossfadeAnimation(AnimationDefinition animDef)
    {
        if(Animation == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(animDef.Animation))
        {
            var thisAnimation = Animation[animDef.Animation];
            thisAnimation.wrapMode = animDef.WrapMode;
            thisAnimation.speed = animDef.Speed;

            if (animDef.Backwards)
            {
                thisAnimation.speed *= -1;
            }

            var asvt = Rm_RPGHandler.Instance.ASVT;
            if(!string.IsNullOrEmpty(animDef.Name) && asvt.UseStatForMovementSpeed && new[]{"Walk, Strafe, Run"}.Any(s => s.Contains(animDef.Name)))
            {
                var asvtStat = asvt.StatForMovementID;
                var bonusMove = _controller.Character.GetStatByID(asvtStat).TotalValue - 1;
                if(bonusMove > 0)
                {
                    var add = 1 + (bonusMove/4);
                    thisAnimation.speed *= add;    
                }
            }

            Animation.CrossFade(animDef.Animation);
        }
        else
        {
            if (!string.IsNullOrEmpty(_controller.LegacyAnimation.IdleAnim.Animation))
                Animation.CrossFade(_controller.LegacyAnimation.IdleAnim.Animation);
        }

        var canLoop = new[] { "Jump", "Knock Back", "Knock Up","Take Hit","Death"}.All(s => s != animDef.Name);
        if (canLoop && animDef.Sound != null)
        {
            var id = animDef.Name;

            if (animDef.Name.Contains("Walk") || animDef.Name.Contains("Run"))
                id = "Walk";
            else if (animDef.Name.Contains("Strafe"))
                id = "Strafe";
            else if (animDef.Name.Contains("Turn"))
                id = "Turn";
            else if (animDef.Name.Contains("Attack"))
                id = "";

            AudioPlayer.Instance.Play(animDef.Sound, AudioType.SoundFX,transform.position, transform, id != "" ? "anim_Core_" + id + "_" + _animationId : "");
        }
    }

    public void CrossfadeAnimation(string animationname, float speed = 1.0f, WrapMode wrapMode = WrapMode.Loop, bool backwards = false)
    {
        var animDef = new AnimationDefinition {Animation = animationname, Speed = speed, WrapMode = wrapMode, Backwards = backwards};
        CrossfadeAnimation(animDef);
    }
}
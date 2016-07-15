using UnityEngine;

using System.Collections;
using System;

public class Fireball : Skill, IAOE, IDamage, IProjectile {
    private float damageMultiplier;
    private float maxDistance;
    private float radius;
    private float speed;


    public Fireball() {
        damageMultiplier = 1;
        maxDistance = 50;
        radius = 1;
        speed = 1;

        //From base skill
        this.Name = "Fireball";
        this.ID = 0;
        this.cost = 5;
    }



    #region interfaces

    public float DamageMultiplier {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public float MaxDistance {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public float Radius {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public float Speed {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    #endregion


}
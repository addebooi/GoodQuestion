using UnityEngine;

using System.Collections;


public abstract class Skill {
    private string name;
    private float cost;
    private int ID;

    /*
        NAME: FIREBALL
        SCALE: 1
        LEVEL : 1
        NRPROJECTILES: 1
        ON COLLISION: NONE / EXPLODE
        RESISTANCE PEN: 0
        CAN HIT MULTIPLE TIMES: NO

    */


    public abstract void Cast(Vector3 mousePos);
}
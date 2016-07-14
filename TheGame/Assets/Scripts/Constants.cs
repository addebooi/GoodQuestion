using UnityEngine;
using System.Collections;

public class Constants {
    public enum entityState {
        IDLING,
        MOVING,
        CASTING,
        DEAD,
        NUMACTIONS
    }

    public enum ENTITYTEAM {
        FRIEDLY,
        NEUTRAL,
        HOSTILE,
        NUMTEAMS
    }

    public enum skillType {
        PROJECTILE,
        NUMSKILLTYPES
    }
}
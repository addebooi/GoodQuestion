using UnityEngine;
using System.Collections;
using System;

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

public class parseEnum<T> {
    public T getEnumFromInt(int index) {
        return (T)Enum.Parse(typeof(T), index.ToString()) ;
    }
}

//public class ArrayInit<T> { 
//    public static T[] CreateArray<T>(uint length, ){
//        T[] returnArr = new T[length];
//        for(uint i = 0; i< length; i++) {
//            returnArr[i] = new T();
//        }

//        return returnArr;
//    }
//}
   
using UnityEngine;

using System.Collections;


public class Enums {
}

public enum StatEnums {
    Strength,
    Stamina,
    Intelligence,
    Dexterity,
    None,
    NumStatEnums
}

public enum AttributeEnums {
    //Damages
    MinDamage,
    MaxDamage,
    CritChance,
    CritMultiplier,
    RangedDamageMulti,
    SpellDamageMulti,
    MelleDamageMulti,
    //Defences
    Health,
    Mana,
    Armour,
    
    None,
    NumAttrEnums
}

public enum ResourceEnums {
    Health,
    Mana,
    NumResourceEnums
}
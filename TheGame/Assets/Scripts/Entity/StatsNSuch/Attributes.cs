using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Stats))]
public class Attributes : MonoBehaviour  {
    BaseAttribute[] attributes;

    parseEnum<AttributeEnums> parser;


    void Awake() {
        parser = new parseEnum<AttributeEnums>();
        attributes = new BaseAttribute[(int)AttributeEnums.NumAttrEnums];
        for (uint i = 0; i < (int)AttributeEnums.NumAttrEnums; i++) { attributes[i] = new BaseAttribute(); }
        //Debug.Log((int)AttributeEnums.NumAttrEnums);

        setupBaseAttributes();
    }


    //public Attributes() {
        
    //}

    public void statUpdated(StatEnums stat, float diffrence) {
        for(int i = 0; i < (int)AttributeEnums.NumAttrEnums; i++) {
            if (attributes[i].hasModifyingType(stat)) {
                float valueBeforeChange = attributes[i].getAttriValue();

                modifyAttribute(parser.getEnumFromInt(i), diffrence);
                //attributes[i].modifyStat(diffrence);

                Debug.Log("Current Attri:" + parser.getEnumFromInt(i).ToString() + " " + valueBeforeChange + " -> " + attributes[i].getAttriValue() );
            }
        }
    }

    public void modifyAttribute(AttributeEnums attribute, float val) {

        attributes[(int)attribute].modifyStat(val);


    }



    private void setupBaseAttributes() {
        //Setup Base Crit Chance %
        attributes[(int)AttributeEnums.CritChance].setValue(5);
        attributes[(int)AttributeEnums.CritChance].setStatModifier(StatEnums.Dexterity, .05f);

        //Setup Base Crit Modifier %
        attributes[(int)AttributeEnums.CritMultiplier].setValue(150);

        //Setup Base Armour
        attributes[(int)AttributeEnums.Armour].setValue(0);
        attributes[(int)AttributeEnums.Armour].setStatModifier(StatEnums.Strength, 1f);

        //Setup Base Damage
        attributes[(int)AttributeEnums.MinDamage].setValue(0);
        attributes[(int)AttributeEnums.MaxDamage].setValue(0);

        //Multiplier Damage
        attributes[(int)AttributeEnums.SpellDamageMulti].setValue(1);
        attributes[(int)AttributeEnums.SpellDamageMulti].setStatModifier(StatEnums.Intelligence, .01f);

        attributes[(int)AttributeEnums.RangedDamageMulti].setValue(1);
        attributes[(int)AttributeEnums.RangedDamageMulti].setStatModifier(StatEnums.Dexterity, .01f);

        attributes[(int)AttributeEnums.MelleDamageMulti].setValue(1);
        attributes[(int)AttributeEnums.MelleDamageMulti].setStatModifier(StatEnums.Strength, .01f);

    }

    public float getAttributeVal(AttributeEnums attri) {
        return attributes[(int)attri].getAttriValue();
    }

    //public AttributeEnums[] getAttributesWhichStatModifies(StatEnums stat) {
    //    AttributeEnums[] returnList;

    //    for (uint i = 0; i< (int)AttributeEnums.NumAttrEnums; i++) {
    //        if (attributes[i].hasModifyingType(stat)) {
    //            ret
    //        }
    //    }


    //    return returnList;
    //} 
}

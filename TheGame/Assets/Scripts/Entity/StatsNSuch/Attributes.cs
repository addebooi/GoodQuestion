using UnityEngine;
using System;

using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(ResourceHandler))]
public class Attributes : MonoBehaviour  {
    BaseAttribute[] attributes;

    ResourceHandler resourceHandler;
    void Start() {

        resourceHandler = GetComponent<ResourceHandler>();
        attributes = new BaseAttribute[(int)AttributeEnums.NumAttrEnums];
        for (uint i = 0; i < (int)AttributeEnums.NumAttrEnums; i++) { attributes[i] = new BaseAttribute(); }
        //Debug.Log((int)AttributeEnums.NumAttrEnums);

        setupBaseAttributes();
    }

    //public Attributes() {
        
    //}

    public void statUpdated(StatEnums stat, float diffrence) {
        for(uint i = 0; i < (int)AttributeEnums.NumAttrEnums; i++) {
            if (attributes[i].hasModifyingType(stat)) {
                float valueBeforeChange = attributes[i].getAttriValue();

                modifyAttribute(i, diffrence);
                //attributes[i].modifyStat(diffrence);

                Debug.Log("Current Attri:" + ((AttributeEnums)Enum.Parse(typeof(AttributeEnums), i.ToString())).ToString() + " " + valueBeforeChange + " -> " + attributes[i].getAttriValue() );
            }
        }
    }

    public void modifyAttribute(AttributeEnums attribute, float val) {
        float beforeChangeAttribute = attributes[(int)attribute].getAttriValue();

        attributes[(int)attribute].modifyStat(val);


        if (beforeChangeAttribute != attributes[(int)attribute].getAttriValue())
            updateResourceIfNecisary(attribute, val);
    }

    public void modifyAttribute(uint attributeIndex, float val) {

        float beforeChangeAttribute = attributes[attributeIndex].getAttriValue();

        attributes[attributeIndex].modifyStat(val);

        if(beforeChangeAttribute != attributes[attributeIndex].getAttriValue())
            updateResourceIfNecisary((AttributeEnums)Enum.Parse(typeof(AttributeEnums), attributeIndex.ToString()), val);
    }

    private void updateResourceIfNecisary(AttributeEnums attribute, float val) {
        if (!isAttriResource(attribute))
            return;

        switch (attribute) {
            case AttributeEnums.Health:
                resourceHandler.modifyMaxValue(ResourceEnums.Health, attributes[(int)attribute].getModifiedValueChange(val));
                break;
            case AttributeEnums.Mana:
                resourceHandler.modifyMaxValue(ResourceEnums.Mana, attributes[(int)attribute].getModifiedValueChange(val));
                break;
        }
        
    }

    private bool isAttriResource(AttributeEnums attribute) {
        bool returnVal = false;

        switch (attribute) {
            case AttributeEnums.Health:
                returnVal = true;
                break;
            case AttributeEnums.Mana:
                returnVal = true;
                break;
        }

        return returnVal;
    }

    private void modifiedResource(ResourceEnums resource, float val) {
        resourceHandler.modifyMaxValue(resource, val);
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

        //Setup Base Health
        attributes[(int)AttributeEnums.Health].setValue(100);
        attributes[(int)AttributeEnums.Health].setStatModifier(StatEnums.Stamina, 5f);

        //Setup Base Mana
        attributes[(int)AttributeEnums.Mana].setValue(100);
        attributes[(int)AttributeEnums.Mana].setStatModifier(StatEnums.Intelligence, 1f);

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

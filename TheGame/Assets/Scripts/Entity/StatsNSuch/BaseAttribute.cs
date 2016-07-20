using UnityEngine;

using System.Collections;


public class BaseAttribute {
    float value;
    float statModifier;
    StatEnums statModifierType;

    public BaseAttribute() {
        value = 0;
        statModifier = 1;
        statModifierType = StatEnums.None;
    }

    public void modifyValue(float val) {
        this.value += val;
    }

    public void setValue(float val) {
        this.value = val;
    }

    public float getAttriValue() {
        return value;
    }

    public void setStatModifier(StatEnums stat, float modifier) {
        this.statModifierType = stat;
        this.statModifier = modifier;
    }

    public bool hasModifyingType(StatEnums stat) {
        return (statModifierType == stat);
    }

    public void modifyStat(float val) {
        value += statModifier * val;
        if (value < 0)
            value = 0;

    }

    public float getModifiedValueChange(float val) {
        return statModifier * val;
    }
    
}
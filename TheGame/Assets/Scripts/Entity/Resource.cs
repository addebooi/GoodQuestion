

using System.Collections;


public class Resource{
    float maxValue;
    float curValue;

    StatEnums statModifierType;
    float statModifier;

    

    public Resource() {
        maxValue = 0;
        curValue = maxValue;
        statModifier = 1;
        statModifierType = StatEnums.None;
    }

    public void modifyMaxValue(float val) {
        maxValue += val;
        if (curValue > maxValue)
            curValue = maxValue;
    }

    public void modifyCurValue(float val) {
        curValue += val;

        checkCurValue();
    }

    public void setMaxValue(float val) {
        maxValue = val;
    }

    public void setCurValue(float val) {
        curValue = val;

        checkCurValue();
    }

    public float getMaxValue() {
        return maxValue;
    }

    public float getCurValue() {
        return curValue;
    }

    private void checkCurValue() {
        if (curValue > maxValue)
            curValue = maxValue;
    }

    public bool hasStatModifier(StatEnums statModi) {
        return this.statModifierType == statModi;
    }

    public void setStatModifier(StatEnums stat, float modifier) {
        this.statModifierType = stat;
        this.statModifier = modifier;
    }

    public float getStatModifiedValue(float val) {
        return val * statModifier;
    }
}
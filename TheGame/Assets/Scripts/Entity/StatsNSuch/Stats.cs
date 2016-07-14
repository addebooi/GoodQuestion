using UnityEngine;

using System.Collections;


public class Stats  {
    BaseStat[] stats;

    public Stats() {
        stats = new BaseStat[(int)StatEnums.NumStatEnums];
        for (uint i = 0; i < (int)StatEnums.NumStatEnums; i++) { stats[i] = new BaseStat(); }
    }

    public void modifyStat(StatEnums stat, float value) {
        stats[(int)stat].modifyValue(value);
    }

    public void setStat(StatEnums stat, float value) {
        stats[(int)stat].setValue(value);
    }

    public float getStatValue(StatEnums stat) {
        return stats[(int)stat].getValue();
    }

}
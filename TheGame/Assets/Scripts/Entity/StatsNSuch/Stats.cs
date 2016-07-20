using UnityEngine;

using System.Collections;
using System;

[RequireComponent(typeof(Attributes))]
public class Stats : MonoBehaviour {
    [SerializeField]
    BaseStat[] stats;

    Attributes attributeRef;

    void Start() {
        attributeRef = GetComponent<Attributes>();

        stats = new BaseStat[(int)StatEnums.NumStatEnums];
        for (uint i = 0; i < (int)StatEnums.NumStatEnums; i++) {
            stats[i] = new BaseStat();
        }
    }

    public void modifyStat(StatEnums stat, float value) {
        stats[(int)stat].modifyValue(value);

        Debug.Log("Current Stat:" + stat.ToString() + " " + (stats[(int)stat].getValue() - value) + " -> " + stats[(int)stat].getValue());

        //Calls attribute that stats has changed
        attributeRef.statUpdated(stat, value);
    }

    public void setStat(StatEnums stat, float value) {
        stats[(int)stat].setValue(value);
    }

    public float getStatValue(StatEnums stat) {
        return stats[(int)stat].getValue();
    }

}
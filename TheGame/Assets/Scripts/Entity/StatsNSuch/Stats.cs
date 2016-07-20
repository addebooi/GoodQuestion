using UnityEngine;

using System.Collections;
using System;

[RequireComponent(typeof(Attributes))]
[RequireComponent(typeof(ResourceHandler))]
public class Stats : MonoBehaviour {
    [SerializeField]
    BaseStat[] stats;

    Attributes attributeRef;
    ResourceHandler resourceHandlerRef;

    parseEnum<StatEnums> parser;

    void Start() {
        attributeRef = GetComponent<Attributes>();
        resourceHandlerRef = GetComponent<ResourceHandler>();
        parser = new parseEnum<StatEnums>();

        stats = new BaseStat[(int)StatEnums.NumStatEnums];
        for (int i = 0; i < (int)StatEnums.NumStatEnums; i++) {
            stats[i] = new BaseStat();

            notifyChange(parser.getEnumFromInt(i), stats[i].getValue());
        }
    }

    public void modifyStat(StatEnums stat, float value) {

        float statBeforeChange = stats[(int)stat].getValue();

        stats[(int)stat].modifyValue(value);

        float diffrenceStat = stats[(int)stat].getValue() - statBeforeChange;

        Debug.Log("Current Stat:" + stat.ToString() + " " + (stats[(int)stat].getValue() - value) + " -> " + stats[(int)stat].getValue());

        //Calls attribute that stats has changed
        notifyChange(stat, diffrenceStat);
        
        
    }

    private void notifyChange(StatEnums stat, float diff) {
        if (diff != 0) {
            attributeRef.statUpdated(stat, diff);
            resourceHandlerRef.statUpdated(stat, diff);
        }
    }

    public void setStat(StatEnums stat, float value) {
        stats[(int)stat].setValue(value);
    }

    public float getStatValue(StatEnums stat) {
        return stats[(int)stat].getValue();
    }

}
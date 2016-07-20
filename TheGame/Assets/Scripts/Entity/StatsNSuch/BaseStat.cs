using UnityEngine;

using System.Collections;


public class BaseStat  {
    [SerializeField]
    float value;

    public BaseStat() {
        value = 20;
    }

    public void modifyValue(float val) {
        this.value += val;
    }

    public void setValue(float val) {
        this.value = val;
    }

    public float getValue() {
        return value;
    }


}
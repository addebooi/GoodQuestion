using UnityEngine;

using System.Collections;


public class BaseStat  {
    float value;

    public BaseStat() {
        value = 0;
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
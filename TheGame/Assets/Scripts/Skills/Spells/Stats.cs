using UnityEngine;

using System.Collections;


public class Stats {
    private float maxHealth;
    private float currentHealth;

    private float maxResource;
    private float currentResource;


    private float Strength;
    private float Dextirity;
    private float Wisdom;


    private float physicalModifier;
    private float elementalModifier;

    private void loadStats() {

        Strength = 8;
        Dextirity = 15;
        Wisdom = 20;

        calculateModifiers();
    }

    private void calculateModifiers() {

        physicalModifier = 1 + ((Strength / 2)/10);
        elementalModifier = 1 + ((Wisdom / 2) / 10);
    }

    public void removeHealth(float amount) {
        this.currentHealth -= amount;
    }

}
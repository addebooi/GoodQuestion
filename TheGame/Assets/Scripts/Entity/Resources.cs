

using System.Collections;


public class Resources {
    private float maxHealth;
    private float currentHealth;

    private float maxResource;
    private float currentResource;

    
    public float CurrentHealth {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public float MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float CurrentResource {
        get { return CurrentResource; }
        set { CurrentResource = value; }
    }

    public float MaxResource {
        get { return MaxResource; }
        set { MaxResource = value; }
    }


    public void addMaxHealth(float health) {
        this.maxHealth += health;
        currentHealth += health;
    }

    public void removeMaxHealth(float health) {
        this.maxHealth -= health;
        if (this.maxHealth < 1) { }
            this.maxHealth = 1;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void addMaxResource(float resource) {
        this.maxHealth += resource;
        currentHealth += resource;
    }

    public void removeMaxResource(float resource) {
        this.maxHealth -= resource;
        if (this.maxHealth < 1) { }
        this.maxHealth = 1;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
using UnityEngine;

using System.Collections;


public class Entity : MonoBehaviour {
    protected Attributes attributes;
    protected Stats stats;
    protected ResourceHandler resources;


    public void InstantiateBase() {
        attributes = new Attributes();
        stats = new Stats();
        resources = new ResourceHandler();
        Debug.Log("in entity");
    }

    public void modifyStat(StatEnums stat, float val) {
        stats.modifyStat(stat, val);
        attributes.statUpdated(stat, val);

        updateRecources();
    }

    private void updateRecources() {
        resources.setResourcesFromAttribute(ResourceEnums.Health, attributes.getAttributeVal(AttributeEnums.Health));

        resources.setResourcesFromAttribute(ResourceEnums.Mana, attributes.getAttributeVal(AttributeEnums.Mana));
    }


    public void takeDamage(float damage) {
        resources.modifyCurValue(ResourceEnums.Health, damage);


        //ENTITY DEAD
        if (!resources.hasOverResource(ResourceEnums.Health, 0)) {

        }
    }



    public bool canCast(ResourceEnums resource, float val) {
        if(resources.hasOverResource(resource, val)) {
            spendResource(resource, val);
            return true;
        }

        return false;
    }

    public void spendResource(ResourceEnums resource, float val) {
        resources.modifyCurValue(resource, -val);
    }






    //// Update is called once per frame
    //	void Update () {
    //        Debug.Log("IN ENTITY");

    //	}



}
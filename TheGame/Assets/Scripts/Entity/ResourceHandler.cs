using UnityEngine;

using System.Collections;


public class ResourceHandler {
    private Resource[] resources;
    private Resource[] resourcesFromAttributes;


    public ResourceHandler() {
        resources = new Resource[(int)ResourceEnums.NumResourceEnums];
        for (uint i = 0; i < (int)ResourceEnums.NumResourceEnums; i++) { resources[i] = new Resource(); }

        resourcesFromAttributes = new Resource[(int)ResourceEnums.NumResourceEnums];
        for (uint i = 0; i < (int)ResourceEnums.NumResourceEnums; i++) { resourcesFromAttributes[i] = new Resource(); }
        setupBasicResources();
        
    }

    public void setResourcesFromAttribute(ResourceEnums resource, float val) {
        resources[(int)resource].modifyMaxValue(-resourcesFromAttributes[(int)resource].getMaxValue());

        resourcesFromAttributes[(int)resource].setMaxValue(val);

        resources[(int)resource].modifyMaxValue(resourcesFromAttributes[(int)resource].getMaxValue());

    }


    private void setupBasicResources() {

        //Health
        resources[(int)ResourceEnums.Health].setMaxValue(50);
        resources[(int)ResourceEnums.Health].setCurValue(resources[(int)ResourceEnums.Health].getMaxValue());

        //Mana
        resources[(int)ResourceEnums.Mana].setMaxValue(10);
        resources[(int)ResourceEnums.Mana].setCurValue(resources[(int)ResourceEnums.Mana].getMaxValue());
    }

    public void modifyMaxValue(ResourceEnums resource, float val) {
        resources[(int)resource].modifyMaxValue(val);
    }

    public void modifyCurValue(ResourceEnums resource, float val) {
        resources[(int)resource].modifyCurValue(val);
    }

    public bool hasOverResource(ResourceEnums resource, float val) {
        return resources[(int)resource].getCurValue()> val;
    }

    public float getResourceMaxVal(ResourceEnums resource) {
        return resources[(int)resource].getMaxValue();
    }

    public float getResourceCurVal(ResourceEnums resource) {
        return resources[(int)resource].getCurValue();
    }



}
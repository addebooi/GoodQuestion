using UnityEngine;

using System.Collections;


public class ResourceHandler : MonoBehaviour {
    private Resource[] resources;
    private Resource[] resourcesFromAttributes;
    parseEnum<ResourceEnums> parser;

    void Awake() {
        parser = new parseEnum<ResourceEnums>();
        resources = new Resource[(int)ResourceEnums.NumResourceEnums];
        for (uint i = 0; i < (int)ResourceEnums.NumResourceEnums; i++) { resources[i] = new Resource(); }

        resourcesFromAttributes = new Resource[(int)ResourceEnums.NumResourceEnums];
        for (uint i = 0; i < (int)ResourceEnums.NumResourceEnums; i++) { resourcesFromAttributes[i] = new Resource(); }
        setupBasicResources();
    }

    public void setResourcesFromAttribute(ResourceEnums resource, float val) {

        //1000 LIV -> 800
        resources[(int)resource].modifyMaxValue(-resourcesFromAttributes[(int)resource].getMaxValue());

        resourcesFromAttributes[(int)resource].setMaxValue(val);

        resources[(int)resource].modifyMaxValue(resourcesFromAttributes[(int)resource].getMaxValue());

    }

    public void statUpdated(StatEnums stat, float val) {
        for (int i = 0; i < (int)ResourceEnums.NumResourceEnums; i++) {
            if (resources[i].hasStatModifier(stat)) {
                modifyMaxValue(parser.getEnumFromInt(i), resources[i].getStatModifiedValue(val));
            }
        }
    }

    private void setupBasicResources() {

        //Health
        resources[(int)ResourceEnums.Health].setMaxValue(50);
        resources[(int)ResourceEnums.Health].setCurValue(resources[(int)ResourceEnums.Health].getMaxValue());
        resources[(int)ResourceEnums.Health].setStatModifier(StatEnums.Stamina, 5);

        //Mana
        resources[(int)ResourceEnums.Mana].setMaxValue(10);
        resources[(int)ResourceEnums.Mana].setCurValue(resources[(int)ResourceEnums.Mana].getMaxValue());
        resources[(int)ResourceEnums.Mana].setStatModifier(StatEnums.Intelligence, 5);
    }

    public void modifyMaxValue(ResourceEnums resource, float val) {

        float valueBeforeChange = resources[(int)resource].getMaxValue();

        resources[(int)resource].modifyMaxValue(val);

        Debug.Log("MAIN!!!: " + resource.ToString() + " " + valueBeforeChange + " -> " + resources[(int)resource].getMaxValue());
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
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Player : Entity {
    Constants.entityState playerState;
    PlayerMovement movement;
    int write = 250;
    int cWrite = 0;

    #region UI
    public Slider Healthbar;
    public Slider Manabar;
    #endregion

    // Use this for initialization

    void Awake() {
        InstantiateBase();
    }

	void Start () {
        
        playerState = Constants.entityState.IDLING;
        movement = GetComponent<PlayerMovement>();

        Debug.Log(this.attributes.getAttributeVal(AttributeEnums.Health));
        
    }
	
	
// Update is called once per frame
	void Update () {
       
        UpdatePlayerAction();
        cWrite += 1;
        //modifyStat(StatEnums.Stamina, 1);

        Debug.Log(resources.getResourceCurVal(ResourceEnums.Health) / resources.getResourceMaxVal(ResourceEnums.Health));
       Healthbar.value =  resources.getResourceCurVal(ResourceEnums.Health) / resources.getResourceMaxVal(ResourceEnums.Health);
        if (cWrite >= write) {
            takeDamage(5);
            cWrite = 0;
            //Debug.Log("Stamina " + stats.getStatValue(StatEnums.Stamina));
            //Debug.Log("AttribLife " + attributes.getAttributeVal(AttributeEnums.Health));
            //Debug.Log("MaxLife " + resources.getResourceMaxVal(ResourceEnums.Health));
            //Debug.Log("CurLife " + resources.getResourceCurVal(ResourceEnums.Health));
        }
            
	}

    //public void Damage(float damage, Vector3 pos, Constants.ENTITYTEAM team) {
    //    stats.removeHealth(damage);

    //    checkDead();
    //}

    private void checkDead() {
        //If dead, do something
    }

    private void UpdatePlayerAction() {
        if(isClearMinded()) {
            movement.UpdateMovement();
        }

    }

    private bool isClearMinded() {
        return playerState != Constants.entityState.CASTING && playerState != Constants.entityState.DEAD;
    }
}
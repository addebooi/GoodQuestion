using UnityEngine;

using System.Collections;


public class Player : BaseCharacter {
    Stats stats;
    Constants.entityState playerState;
    PlayerMovement movement;

	// Use this for initialization
	void Start () {
        stats = new Stats();
        playerState = Constants.entityState.IDLING;
        movement = GetComponent<PlayerMovement>();
    }
	
	
// Update is called once per frame
	void Update () {
        UpdatePlayerAction();
	    
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
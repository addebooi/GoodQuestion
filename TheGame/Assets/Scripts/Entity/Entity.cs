using UnityEngine;

using System.Collections;


public class Entity : MonoBehaviour, IDamageAble {
    public float Health;

	// Use this for initialization
	void Start () {
        Health = 40;
	
	}
	
	
// Update is called once per frame
	void Update () {

	
	}

    public void Damage(float damage) {
        Health -= damage;
        checkDead();


    }

    private void checkDead() {
        if (Health <= 0) {
            Destroy(gameObject);
        }
    }

}
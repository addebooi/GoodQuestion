
using UnityEngine;

using System.Collections;


public abstract class Entity : MonoBehaviour, IDamageAble {
    


	// Use this for initialization
	void Start () {

	
	}
	
	
// Update is called once per frame
	void Update () {

	
	}

    public void Damage(float damage, Vector3 pos, Constants.ENTITYTEAM team) {

    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionHandler : MonoBehaviour {
    List<GameObject> objectsWithCollision = new List<GameObject>();
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        checkCollision();
	}

    void checkCollision() {
        for(int i = 0; i < objectsWithCollision.Count; i++) {
            for (int k = 1+i; k < objectsWithCollision.Count; k++) {

                if (objectsWithCollision[i].GetComponent<Collider>().getCollider().Overlaps((objectsWithCollision[k].GetComponent<Collider>().getCollider()))){

                    print("Colliding!");
                }

            }
        }
    }

    public void addGameObject(GameObject obj) {
        this.objectsWithCollision.Add(obj);
    }
    
}

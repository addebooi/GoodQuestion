using UnityEngine;
using System.Collections;

public class DieAfterXSeconds : MonoBehaviour {
    public float maxAliveTime;
    private float currentAliveTime;

	// Use this for initialization
	void Start () {
        currentAliveTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentAliveTime += Time.deltaTime;

        if(currentAliveTime >= maxAliveTime) {
            Destroy(gameObject);
        }
	}
}

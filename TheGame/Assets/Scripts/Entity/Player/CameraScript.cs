using UnityEngine;

using System.Collections;


public class CameraScript : MonoBehaviour {
    private Transform Player;
    private Vector3 distanceFromPlayer;
    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        distanceFromPlayer =new Vector3(0, 14, -5);
    }
	
	
// Update is called once per frame
	void Update () {
        transform.position = Player.position + distanceFromPlayer;
	}

}
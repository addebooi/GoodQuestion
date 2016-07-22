using UnityEngine;
using System.Collections;

    

public class BasicSkill : MonoBehaviour {

    Attributes senderAttributes;
    Vector3 senderPosition;
    Vector3 senderDirection;

    public void Instantiate(Attributes attri, Vector3 pos, Vector3 dir) {
        senderAttributes = attri;
        senderDirection = dir;
        senderPosition = pos;
    }


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Attributes getSenderAttributes() {
        return senderAttributes;
    }

    public Vector3 getSenderPosition() {
        return senderPosition;
    }

    public Vector3 getSenderDirection() {
        return senderDirection;
    }


}

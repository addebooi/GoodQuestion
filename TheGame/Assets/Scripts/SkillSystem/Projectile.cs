using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BasicSkill))]
public class Projectile : MonoBehaviour {
    public float baseSpeed;
    public Vector3 direction;

    public Vector3 velocity;

    BasicSkill basicSkillRef;

    //To implement
    //Player can modify projectile speed


	// Use this for initialization
	void Start () {
        basicSkillRef = GetComponent<BasicSkill>();
        readFromDirection();
	}

    private void readFromAttributes() {
        
    }

    private void readFromDirection() {
        this.direction = basicSkillRef.getSenderDirection();
        this.velocity = direction * baseSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(this.velocity * Time.deltaTime);
	}
}

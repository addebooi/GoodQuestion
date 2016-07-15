using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public struct damageParams {
    public float Damage;
    public Vector3 position;
    public Vector3 velocity;
    public float speed;
    public damageParams(float damage, Vector3 position, Vector3 velocity, float speed) {
        this.Damage = damage;
        this.position = position;
        this.speed = speed;
        this.velocity = velocity;
    }
}

public class playerSpellHandler : MonoBehaviour {
    //List<Skill> skills;
    public List<GameObject> spellPrefabs;

	// Use this for initialization
	void Start () {
        //skills = new List<Skill>();
        //skills.Add(new Fireball());
	    
	}
	
	
// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500f)){
                GameObject cube = (GameObject)Instantiate(spellPrefabs[0], transform.position, Quaternion.identity);
                cube.SendMessage("ActivateSkill", new damageParams(2, transform.position,
                    Vector3.Normalize(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position), 20));
            }
            
        }
	
	}

}
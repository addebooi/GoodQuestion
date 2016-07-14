using UnityEngine;

using System.Collections;


public class Fireball : MonoBehaviour {
    public Vector3 velocity;
    public float speed;
    public float damage;
    public float ID;


    // Use this for initialization
    void Start() {
        //this.velocity = new Vector3(0, 0, 1);
        //this.speed = 5;
        //this.damage = 10;
        //ID = 1;

    }


    // Update is called once per frame
    void Update() {
        this.transform.position += this.velocity * speed * Time.deltaTime;
    }

    public void ActivateSkill(damageParams param) {
        this.velocity = param.velocity;
        this.speed = param.speed;
        this.damage = param.Damage;
        transform.position = param.position;
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.tag != "Player") {
            Debug.Log("collided with: " + other.transform.name);
            other.SendMessage("Damage", damage);
            disableEmitters(transform);

            transform.DetachChildren();



            //GetComponent<ParticleSystem>()= false;
            Destroy(gameObject);
        }
        
    }

    void disableEmitters(Transform transform) {
        for(int i = 0; i< transform.childCount; i++) {
            
            disableEmitters(transform.GetChild(i));
            //if (transform.GetChild(i).childCount > 0) {
                
            //}
        }
        ParticleSystem derp = transform.GetComponent<ParticleSystem>();
        if (derp != null)
            derp.Stop();
    }
}
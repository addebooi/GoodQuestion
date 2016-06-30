using UnityEngine;

using System.Collections;


public class PlayerMovement : MonoBehaviour {
    public NavMeshAgent agent;
    public int layer;
    private bool pathing;

    void Start() {
        this.agent = GetComponent<NavMeshAgent>();
        layer = LayerMask.GetMask("Floor");
       // agent.updateRotation = false;
        agent.speed = 7f;
        pathing = false;
    }

    void Update() {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction* 50, Color.gray);
        if (Input.GetMouseButtonDown(0)) {
            
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500f,layer)) {
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.SetDestination(hit.point);
                
                //agent.SetDestination(hit.point);
            }
        }
        if (!agent.hasPath && pathing) {
            pathing = false;
            agent.updatePosition = false;
            agent.updateRotation = false;
        }
            
        if (agent.hasPath) {
            //Debug.DrawRay(transform.position, Vector3.Normalize(agent.nextPosition- transform.position) * 50, Color.gray);
            //Debug.Log(agent.nextPosition);
            //transform.LookAt(new Vector3(agent.destination.x, transform.position.y, agent.destination.z));// = Quaternion.FromToRotation(transform.position, agent.nextPosition);
           // agent.acceleration = (agent.remainingDistance < 1) ? agent.acceleration : 60;
        }

    }


}
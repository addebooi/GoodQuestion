using UnityEngine;

using System.Collections;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour {
    public NavMeshAgent agent;
    public int layer;
    void Start() {
        this.agent = GetComponent<NavMeshAgent>();
        layer = LayerMask.GetMask("Floor");
        agent.speed = 7f;
    }

    public void UpdateMovement() {

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) {
            onMouseDown();
        }
        if (Input.GetMouseButtonUp(0)) {
            onMouseUp();
        }
    }

    private void onMouseDown() {
        KeyValuePair<bool, RaycastHit> hit = getMousePositionPressed();

        if (hit.Key) {
            agent.SetDestination(transform.position + Vector3.Normalize(hit.Value.point - transform.position));
        }
    }

    private void onMouseUp() {
        KeyValuePair<bool, RaycastHit> hit = getMousePositionPressed();

        if (hit.Key) {
            agent.SetDestination(hit.Value.point);
        }
    }

    KeyValuePair<bool, RaycastHit> getMousePositionPressed() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500f, layer)) {
            return new KeyValuePair<bool, RaycastHit>(true, hit);
        }

        return new KeyValuePair<bool, RaycastHit>(false, hit);
    }

    //void Update() {
       

    //}


}
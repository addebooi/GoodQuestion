using UnityEngine;
using System.Collections;

public class Collider : MonoBehaviour {
    public Rect hitBox;
	// Use this for initialization
	void Start () {

        GameObject.FindGameObjectWithTag("ColHandler").GetComponent<CollisionHandler>().addGameObject(this.gameObject);
        calulateSize();

        //print("MINX: " + minX + " MAXX: " + maxX + " MINZ : " + minZ + " MAXZ: " + maxZ);
        //hitBox = new Rect(new Vector2(transform.position.x, transform.position.z),
        //                  new Vector2(maxX-minX, maxZ - minZ));
	}
	
	// Update is called once per frame
	void Update () {
        setPosition();

        
	}

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(new Vector3(hitBox.position.x, 0, hitBox.position.y) + (new Vector3(hitBox.size.x, 0, hitBox.size.y) / 2), new Vector3(hitBox.size.x, .2f, hitBox.size.y));
    }

    void setPosition() {
        this.hitBox.position = transform.position;
    }

    void calulateSize() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;


        float minX = float.MaxValue, maxX = -float.MaxValue, minZ = float.MaxValue, maxZ = -float.MaxValue;

        Vector3 tempVal;
        for (int i = 0; i < vertices.Length; i++) {
            tempVal = vertices[i];
            tempVal.x *= transform.localScale.x;
            tempVal.z *= transform.localScale.z;

            if (tempVal.x < minX)
                minX = tempVal.x;
            if (tempVal.x > maxX)
                maxX = tempVal.x;

            if (tempVal.z < minZ)
                minZ = tempVal.z;
            if (tempVal.z > maxZ)
                maxZ = tempVal.z;
        }
        hitBox.size = new Vector2(maxX - minX, maxZ - minZ);
    }

    public Rect getCollider() {
        return hitBox;
    }
}

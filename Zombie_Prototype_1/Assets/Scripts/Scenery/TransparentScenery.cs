using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentScenery : MonoBehaviour
{
    public Player playerPos;

    public Material material;
    public Material newMaterial;

    public LayerMask transparentLayer;

    public float transparency;
    public float transparencySmoothing;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        playerPos = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Cast a line from player to camera, and if intersects with an object on the TransparentScenery layer, make object's material alpha transparency
        if (Physics.Linecast(Camera.main.transform.position, playerPos.transform.position, out hit, transparentLayer))
        {
            hit.collider.gameObject.GetComponent<MeshRenderer>().material = newMaterial;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(Camera.main.transform.position, playerPos.transform.position);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float rotateSpeed;
    public float bobAmount;
    public float bobSmoothing;

    public bool rotateX, rotateY, rotateZ;
    public bool bobY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bobY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * bobSmoothing, bobAmount), transform.localPosition.z);
        }

        if (rotateX)
        {
            transform.Rotate(rotateSpeed, 0, 0);
        }

        if (rotateY)
        {
            transform.Rotate(0, rotateSpeed, 0);
        }

        if (rotateZ)
        {
            transform.Rotate(0, 0, rotateSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    public Shooting shooting;

    public float fireRate = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        shooting = FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        shooting.timeBetweenShots = fireRate;
    }
}

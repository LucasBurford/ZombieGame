using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public float armourAmount;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Player>().IncreaseCurrentArmour(armourAmount);
        FindObjectOfType<AudioManager>().Play("PickUpArmour");
        Destroy(gameObject);
    }
}

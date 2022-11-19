using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssaultRifleCollect : MonoBehaviour
{
    public TMP_Text pickUpItemText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpItemText.gameObject.SetActive(true);
            pickUpItemText.text = "F: Pick up Assault Rifle?";

            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<Shooting>().assaultRifleAcquired = true;
                FindObjectOfType<ItemAcquired>().AcquiredItem("Assault Rifle");
                FindObjectOfType<AudioManager>().Play("AssaultRifleReload");
                FindObjectOfType<AudioManager>().Play("PickUpItem1");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpItemText.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotgunCollect : MonoBehaviour
{
    public TMP_Text pickUpItemText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpItemText.gameObject.SetActive(true);
            pickUpItemText.text = "F: Pick up Shotgun?";

            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<Shooting>().shotGunAcquired = true;
                FindObjectOfType<ItemAcquired>().AcquiredItem("Shotgun");
                FindObjectOfType<AudioManager>().Play("ShotGunReload");
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

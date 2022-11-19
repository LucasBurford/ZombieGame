using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachineGunCollect : MonoBehaviour
{
    public TMP_Text pickUpItemText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpItemText.gameObject.SetActive(true);
            pickUpItemText.text = "F: Pick up Machine Gun?";

            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<Shooting>().machineGunAcquired = true;
                FindObjectOfType<ItemAcquired>().AcquiredItem("Machine Gun");
                FindObjectOfType<AudioManager>().Play("MachineGunReload");
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

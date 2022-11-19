using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AxeCollect : MonoBehaviour
{
    public TMP_Text pickUpItemText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpItemText.gameObject.SetActive(true);
            pickUpItemText.text = "F: Pick up Axe?";

            if (Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<Shooting>().axeAcquired = true;
                FindObjectOfType<ItemAcquired>().AcquiredItem("Axe");
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

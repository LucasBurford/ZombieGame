using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUpgrade : MonoBehaviour
{
    public Player player;
    public TMP_Text useText;

    public float distanceToActivate;
    public float cost;
    public int increase;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= distanceToActivate)
        {
            useText.gameObject.SetActive(true);
            useText.text = "F: Purchase Armour Upgrade? (" + cost.ToString() + ")";

            if (Input.GetKeyDown(KeyCode.F))
            {

                if (player.score >= cost)
                {
                    player.DecreaseCurrentScore(cost);
                    FindObjectOfType<Shooting>().maxAmmo += increase;
                    FindObjectOfType<AudioManager>().Play("VendingMachine");
                    FindObjectOfType<AudioManager>().Play("PickUpItem1");
                }
                else
                {
                    print("Not enough points!");
                }
            }
        }
        else
        {
            useText.gameObject.SetActive(false);
        }
    }
}

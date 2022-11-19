using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingTable : MonoBehaviour
{
    public Player player;
    public TMP_Text useText;

    public float distanceToActivate;
    public float armourCost;
    public float armourIncrease;

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
            useText.text = "F: Purchase Armour? (1000)";

            if (Input.GetKeyDown(KeyCode.F))
            {

                if (FindObjectOfType<Player>().score >= armourCost)
                {
                    FindObjectOfType<Player>().IncreaseCurrentArmour(armourIncrease);
                    FindObjectOfType<Player>().DecreaseCurrentScore(armourCost);
                    FindObjectOfType<AudioManager>().Play("CraftingTable");
                    FindObjectOfType<AudioManager>().Play("PickUpArmour");
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

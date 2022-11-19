using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VendingMachine : MonoBehaviour
{
    public Player player;
    public TMP_Text useText;

    public float distanceToActivate;
    public float colaDrinkCost;

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
            useText.text = "F: Purchase Cola Drink? (500)";

            if (Input.GetKeyDown(KeyCode.F))
            {

                if (FindObjectOfType<Player>().score >= colaDrinkCost)
                {
                    FindObjectOfType<Inventory>().AddColaDrink();
                    FindObjectOfType<Player>().DecreaseCurrentScore(colaDrinkCost);
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

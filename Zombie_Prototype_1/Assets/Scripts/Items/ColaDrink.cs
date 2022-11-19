using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColaDrink : MonoBehaviour
{
    public Player player;
    public Inventory playerInventory;

    public float healAmount;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("PickUpItem1");
            playerInventory.AddColaDrink();
            Destroy(gameObject);
        }
    }
}

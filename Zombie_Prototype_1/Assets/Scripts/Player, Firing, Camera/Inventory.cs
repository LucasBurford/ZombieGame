using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int colaDrinks;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddColaDrink()
    {
        colaDrinks++;
    }

    public void RemoveColaDrink()
    {
        colaDrinks--;
    }
}

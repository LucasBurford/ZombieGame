using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Inventory playerInventory;

    public Slider healthSlider;
    public Slider armourSlider;

    public TMP_Text healthText;
    public TMP_Text armourText;
    public TMP_Text scoreText;
    public TMP_Text scoreIncreaseText;

    public float maxHealth;
    public float currentHealth;
    public float maxArmour;
    public float currentArmour;
    public float armourFactor;
    public float score;
    float scoreIncrease;
    public float scoreIncreaseTimer; // Time left to add to score before text goes away
    public float scoreIncreaseRaise; // Amount added back onto scoreIncreaseTimer

    public float colaHealAmount;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GetComponent<Inventory>();

        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        armourSlider.maxValue = maxArmour;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        GetInput();

        if (scoreIncreaseTimer <= 0)
        {
            scoreIncreaseTimer = 0;
            StartCoroutine(WaitToRemoveScoreIncreaseText());
        }
        else
        {
            scoreIncreaseTimer -= 0.05f;
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory.colaDrinks > 0)
            {
                if (currentHealth < maxHealth)
                {
                    UseColaDrink();
                }
                else
                {
                    print("I'm good");
                }
            }
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        // Total damage that currentHealth will be reduced by
        float totalDamage;

        // If player has armour
        if (currentArmour > 0)
        {
            // Calculate totalDamage to subtract from currentHealth by deducting armourFactor from incomingDamage
            totalDamage = incomingDamage - armourFactor;

            // Reduce currentArmour by incomingDamage
            currentArmour -= incomingDamage;

            // Play armour clang sound
            FindObjectOfType<AudioManager>().Play("ArmourHit1");
            FindObjectOfType<AudioManager>().Play("PlayerGrunt1");
        }
        // Else if they don't have any armour
        else
        {
            // Keep totalDamage equal to incomingDamage
            FindObjectOfType<AudioManager>().Play("PlayerHurt1");
            totalDamage = incomingDamage;
        }

        // Subtract totalDamage from currentHealth to damage the player
        currentHealth -= totalDamage;
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
    }

    public void IncreaseCurrentArmour(float increaseAmount)
    {
        currentArmour += increaseAmount;
    }

    public void AddToScore(float amount)
    {
        scoreIncreaseText.gameObject.SetActive(true);

        scoreIncrease += amount;
        
        if (scoreIncreaseTimer <= 3)
        {
            scoreIncreaseTimer += scoreIncreaseRaise;
        }
    }

    public void DecreaseCurrentScore(float amount)
    {
        scoreIncreaseText.gameObject.SetActive(true);
        score -= amount;
    }

    private void UpdateUI()
    {
        healthText.text = currentHealth.ToString();
        healthSlider.value = currentHealth;
        armourText.text = currentArmour.ToString();
        armourSlider.value = currentArmour;
        scoreText.text = score.ToString();
        scoreIncreaseText.text = scoreIncrease.ToString();
    }

    private void UseColaDrink()
    {
        float difference = maxHealth - currentHealth;

        // If missing health is less than colaHealAmount
        if (difference < colaHealAmount)
        {
            // Only heal for that amount
            Heal(difference);
        }
        else
        {
            Heal(colaHealAmount);
        }
        FindObjectOfType<AudioManager>().Play("ColaDrinkHeal");
        playerInventory.RemoveColaDrink();
    }

    IEnumerator WaitToRemoveScoreIncreaseText()
    {
        yield return new WaitForSeconds(scoreIncreaseTimer);
        score += scoreIncrease;
        scoreIncrease = 0;
        scoreIncreaseText.gameObject.SetActive(false);
    }
}

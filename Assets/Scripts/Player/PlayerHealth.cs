using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentHealth;
    public int maxHealth = 3;
    public TextMeshProUGUI healthText;
    void Start()
    {
        currentHealth = maxHealth - 1;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDie();
        }
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        //TODO: Change this to use heart sprites instead of texts
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
        else
        {
            Debug.Log("healthText does not exist");
        }
    }
    private void PlayerDie()
    {
        //TODO: Add logic to player death sequence
        Debug.Log("Game over");
    }
}

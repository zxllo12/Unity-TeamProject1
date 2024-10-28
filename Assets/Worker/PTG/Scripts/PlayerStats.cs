using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public float attackPower = 10f; 
    public float defense = 5f; 
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerStats()
    {
        currentHealth = maxHealth;
    }

    //데미지 계산
    public void TakeDamage(float damage)
    {
        float actualDamage = damage - defense;
        actualDamage = Mathf.Clamp(actualDamage, 0, actualDamage);
        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //사망
    private void Die()
    {
        Debug.Log("Player has died.");  
    }
}


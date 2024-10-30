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

    public float invincibleDuration = 1.5f; // 무적 시간 (초)
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    //체력 초기화
    public PlayerStats()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 무적 시간 관리
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    //데미지 계산
    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return; // 무적 상태일 때는 피해 무시

        float actualDamage = damage - defense;
        actualDamage = Mathf.Clamp(actualDamage, 0, actualDamage);
        currentHealth -= actualDamage;

        invincibleTimer = invincibleDuration;
        isInvincible = true;
        
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


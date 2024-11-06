using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerStats
{
    public float attackPower = 10f;
    public float defense = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    public float invincibleDuration = 1.5f; // ���� �ð� (��)
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    public UnityAction OnChangedHP;

    public UnityAction Dead;

    string HitAudioClip = "PlayerHit";

    //ü�� �ʱ�ȭ
    public PlayerStats()
    {
        //currentHealth = maxHealth;
    }

    //���� �ð�
    public void UpdateInvincibleTime(float time)
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    //������ ���
    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return; // ���� ������ ���� ���� ����
        }

        float actualDamage = damage - defense;
        actualDamage = Mathf.Clamp(actualDamage, 0, actualDamage);
        currentHealth -= actualDamage;

        if (!string.IsNullOrEmpty(HitAudioClip))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, HitAudioClip);

        OnChangedHP?.Invoke();

        invincibleTimer = invincibleDuration;
        isInvincible = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //ȸ��
    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += maxHealth * 1f;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // ü���� �ִ�ġ�� ���� �ʵ��� ����
            OnChangedHP?.Invoke();
        }
    }

    //���
    private void Die()
    {
        Dead?.Invoke();
    }
}


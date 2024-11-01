using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUpgradeShopUI : MonoBehaviour
{
    public int upgradedAtk;
    public int upgradedDef;
    public int upgradedHealth;
    public int upgradedCooldown;

    [Header("업그레이드 비용")]
    public int attackPrice;
    public int defensePrice;
    public int healthPrice;
    public int cooldownPrice;

    [Header("UI 참조")]
    public TextMeshProUGUI attackPriceText;
    public TextMeshProUGUI attackIncreaseText;
    public TextMeshProUGUI defensePriceText;
    public TextMeshProUGUI defenseIncreaseText;
    public TextMeshProUGUI healthPriceText;
    public TextMeshProUGUI healthIncreaseText;
    public TextMeshProUGUI cooldownPriceText;
    public TextMeshProUGUI cooldownIncreaseText;
    public TextMeshProUGUI goldText;

    private void Start()
    {
        SetPrice();
        UpdateUI();
    }

    public void UpgradeAttack()
    {
        if (GameManager.Instance.HasEnoughGold(attackPrice))
        {
            GameManager.Instance.SpendGold(attackPrice);
            upgradedAtk++;
            SetPrice();
            UpdateUI();
        }
    }

    public void UpgradeDefense()
    {
        if (GameManager.Instance.HasEnoughGold(defensePrice))
        {
            GameManager.Instance.SpendGold(defensePrice);
            upgradedDef++;
            SetPrice();
            UpdateUI();
        }
    }

    public void UpgradeHealth()
    {
        if (GameManager.Instance.HasEnoughGold(healthPrice))
        {
            GameManager.Instance.SpendGold(healthPrice);
            upgradedHealth++;
            SetPrice();
            UpdateUI();
        }
    }

    public void UpgradeCooldown()
    {
        if (GameManager.Instance.HasEnoughGold(cooldownPrice))
        {
            GameManager.Instance.SpendGold(cooldownPrice);
            upgradedCooldown++;
            SetPrice();
            UpdateUI();
        }
    }

    public void SetPrice()
    {
        attackPrice = upgradedAtk * 10;
        defensePrice = upgradedDef * 10;
        healthPrice = upgradedHealth * 10;
        cooldownPrice = upgradedCooldown * 10;
    }


    private void UpdateUI()
    {
        attackPriceText.text = $"Attack Price: {attackPrice}";
        attackIncreaseText.text = $"공격력 {upgradedAtk * 1} +1";
        defensePriceText.text = $"Defense Price: {defensePrice}";
        defenseIncreaseText.text = $"방어력 {upgradedDef * 0.5f} +0.5";
        healthPriceText.text = $"Health Price: {healthPrice}";
        healthIncreaseText.text = $"체력 {upgradedHealth * 5} +5";
        cooldownPriceText.text = $"Cooldown Price: {cooldownPrice}";
        cooldownIncreaseText.text = $"쿨타임 감소 {upgradedCooldown * 1}% +1%";
        goldText.text = $"Gold: {GameManager.Instance.GetGold()}";
    }

}

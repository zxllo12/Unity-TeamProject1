using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    [SerializeField] Button confirmButton, cancelButton;
    [SerializeField] TextMeshProUGUI skillname, description, element, power, cooltime, range;
    int skillID;

    private void Start()
    {
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(() => UnlockSkillInShop());
    }

    public void SetInfo(int skillID)
    {
        this.skillID = skillID;
        skillname.text = $"{DataManager.Instance.SkillDict[skillID].Name}";
        description.text = $"{DataManager.Instance.SkillDict[skillID].Description}";
        element.text = $"{DataManager.Instance.SkillDict[skillID].SkillType}";
        power.text = $"{DataManager.Instance.SkillDict[skillID].Damage}";
        cooltime.text = $"{DataManager.Instance.SkillDict[skillID].CoolTime}";
        range.text = $"{DataManager.Instance.SkillDict[skillID].Range}";
    }

    public void UnlockSkillInShop()
    {
        int cost = DataManager.Instance.SkillDict[skillID].Price;

        if (GameManager.Instance.HasEnoughGold(cost))
        {
            GameManager.Instance.SpendGold(cost);
            SkillUnlockManager.Instance.UnlockSkill(skillID);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
}

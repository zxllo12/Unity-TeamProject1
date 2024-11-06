using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    [SerializeField] Button confirmButton, cancelButton;
    [SerializeField] TextMeshProUGUI skillname, description, element, power, cooltime, range, resultText;
    [SerializeField] GameObject resultWindow;
    int skillID;

    private void Start()
    {
        cancelButton.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
        });
        confirmButton.onClick.AddListener(() => 
        { 
            UnlockSkillInShop(); 
            gameObject.SetActive(false);
        });
    }

    public void SetInfo(int skillID)
    {
        this.skillID = skillID;
        skillname.text = $"{DataManager.Instance.SkillDict[skillID].Name}";
        description.text = $"{DataManager.Instance.SkillDict[skillID].Description}";
        element.text = $"�Ӽ� : {DataManager.Instance.SkillDict[skillID].SkillType}";
        power.text = $"���� : {DataManager.Instance.SkillDict[skillID].Damage * 100}%";
        cooltime.text = $"�⺻ ��Ÿ�� : {DataManager.Instance.SkillDict[skillID].CoolTime}��";
        range.text = $"���� : {DataManager.Instance.SkillDict[skillID].Range}";
    }

    public void UnlockSkillInShop()
    {
        int cost = DataManager.Instance.SkillDict[skillID].Price;

        if (SkillUnlockManager.Instance.IsSkillUnlocked(skillID))
        {
            Debug.Log("�̹� �رݵ� ��ų�Դϴ�.");
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
            resultWindow.SetActive(true);
            resultText.text = "�̹� �رݵ� ��ų�Դϴ�.";
        }
        else if (GameManager.Instance.HasEnoughGold(cost))
        {
            GameManager.Instance.SpendGold(cost);
            SkillUnlockManager.Instance.UnlockSkill(skillID);
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "Coins");
            resultWindow.SetActive(true);
            resultText.text = $"{DataManager.Instance.SkillDict[skillID].Name} ��/�� �رݵǾ����ϴ�.";
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PlayButton");
            resultWindow.SetActive(true);
            resultText.text = "��尡 �����մϴ�.";
        }
    }
}

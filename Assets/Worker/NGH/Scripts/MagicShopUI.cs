using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicShopUI : MonoBehaviour
{
    // 마법 상점 UI 요소
    [Header("MagicShop UI")]
    public Button magicShopExit;
    public Button[] skillButtons; // 상점 UI에 있는 각 스킬 버튼 배열
    public int[] skillCosts;      // 각 스킬의 해금 비용

    private void Start()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int skillID = i;
            skillButtons[i].onClick.AddListener(() => UnlockSkillInShop(skillID));
        }
    }

    private void UnlockSkillInShop(int skillID)
    {
        int cost = skillCosts[skillID];

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

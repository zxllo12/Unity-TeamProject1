using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RewardChest : MonoBehaviour
{
    public int dataIndex; // 사용할 테이블 인덱스
    public GameObject hpItemPrefab, skillPrefab, goldPrefab, rareGoldPrefab;

    public bool testBool;

    private void Awake()
    {
    }

    private void Start()
    {
        DataManager.Instance.OnLoadCompleted += Test;
    }

    private void Test()
    {
        for (int i = 0; DataManager.Instance.SkillDict.Count > i; i++)
        {
            SkillUnlockManager.Instance.UnlockSkill(i);
        }
        OpenChest();
    }

    private void OnDisable()
    {
        DataManager.Instance.OnLoadCompleted -= Test;
    }

    public void OpenChest()
    {
        DropData selectedTable = DataManager.Instance.DropDict[dataIndex];
        GenerateRewards(selectedTable);
    }

    private void GenerateRewards(DropData table)
    {
        // 골드 생성
        int goldAmount = Random.Range(table.MinGold, table.MaxGold);
        GameObject goldDrop = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        goldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, goldAmount);

        // HP템 드랍 여부 확인
        if (table.IsDropHP)
        {
            GameObject potionDrop = Instantiate(hpItemPrefab, transform.position, Quaternion.identity);
            potionDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Potion,0);
        }

        List<int> availableLowSkills = new List<int>();
        List<int> availableMidSkills = new List<int>();
        List<int> availableHighSkills = new List<int>();

        // 해금된 스킬들을 대상으로 반복
        for (int i= 0; i < SkillUnlockManager.Instance.unlockedSkills.Count; i++)
        {
            Enums.Grade grade = DataManager.Instance.SkillDict[SkillUnlockManager.Instance.unlockedSkills[i]].Grade;
            switch(grade)
            {
                case Enums.Grade.Low :
                    availableLowSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                        break;
                case Enums.Grade.Mid:
                    availableMidSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                    break;
                case Enums.Grade.High:
                    availableHighSkills.Add(SkillUnlockManager.Instance.unlockedSkills[i]);
                    break;
            }
        }

        // 스킬 ID 리스트 (0~8까지)에서 QWER에 등록되지 않은 스킬만 선택하기 위해 목록을 구성합니다.

        // TEST
        List<int> equippedSkills = new List<int>();
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] != null)
            {
                equippedSkills.Add(GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.ID);
            }
        }
        availableLowSkills.RemoveAll(skill => equippedSkills.Contains(skill));
        availableMidSkills.RemoveAll(skill => equippedSkills.Contains(skill));
        availableHighSkills.RemoveAll(skill => equippedSkills.Contains(skill));
        //

        // 스킬 드랍 확률 체크
        float skillRoll = Random.value;
        if (skillRoll <= table.LowGradePercent && availableLowSkills.Count > 0)
        {
            // lowSkill 중에서 랜덤 선택
            int skillID = availableLowSkills[Random.Range(0, availableLowSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent && availableMidSkills.Count > 0)
        {
            // midSkill 중에서 랜덤 선택
            int skillID = availableMidSkills[Random.Range(0, availableMidSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent + table.HighGradePercent && availableHighSkills.Count > 0)
        {
            // highSkill 중에서 랜덤 선택
            int skillID = availableHighSkills[Random.Range(0, availableHighSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }

        // 뽕맛 골드 드랍 체크
        if (Random.value <= table.BonusGoldPercent)
        {
            Instantiate(rareGoldPrefab, transform.position, Quaternion.identity);
        }
    }

    private void CreateSkill(DropItem.ItemType skillType, int skillID)
    {
        GameObject skillDrop = Instantiate(skillPrefab, transform.position, Quaternion.identity);
        skillDrop.GetComponent<DropItem>().Initialize(skillType, skillID);
    }
}
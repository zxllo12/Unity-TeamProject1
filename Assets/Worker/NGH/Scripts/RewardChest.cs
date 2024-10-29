using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RewardChest : MonoBehaviour
{
    public List<DropTable> dropTableList; // 테이블 리스트
    public int tableIndex; // 사용할 테이블 인덱스
    public GameObject hpItemPrefab, skillPrefab, goldPrefab, rareGoldPrefab;

    private void Awake()
    {
        
    }

    public void OpenChest()
    {
        DropTable selectedTable = dropTableList[tableIndex];
        GenerateRewards(selectedTable);
    }

    private void GenerateRewards(DropTable table)
    {
        // 골드 생성
        int goldAmount = Random.Range(table.minGold, table.maxGold);
        GameObject goldDrop = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        goldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, goldAmount);

        // HP템 드랍 여부 확인
        if (table.hasHPItem)
        {
            Instantiate(hpItemPrefab, transform.position, Quaternion.identity);
        }

        // 스킬 ID 리스트 (0~8까지)에서 QWER에 등록되지 않은 스킬만 선택하기 위해 목록을 구성합니다.
        List<int> availableLowSkills = new List<int> { 0, 1, 2 };
        List<int> availableMidSkills = new List<int> { 3, 4, 5 };
        List<int> availableHighSkills = new List<int> { 6, 7, 8 };

        // QWER에 등록된 스킬 ID를 가져와서 위의 목록에서 제거합니다.
        List<int> registeredSkills = UIManager.Instance.GetRegisteredSkills(); // QWER 스킬 리스트 함수

        availableLowSkills.RemoveAll(skill => registeredSkills.Contains(skill));
        availableMidSkills.RemoveAll(skill => registeredSkills.Contains(skill));
        availableHighSkills.RemoveAll(skill => registeredSkills.Contains(skill));

        // 스킬 드랍 확률 체크
        float skillRoll = Random.value;
        if (skillRoll <= table.lowSkillChance && availableLowSkills.Count > 0)
        {
            // lowSkill 중에서 랜덤 선택
            int skillID = availableLowSkills[Random.Range(0, availableLowSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.lowSkillChance + table.midSkillChance && availableMidSkills.Count > 0)
        {
            // midSkill 중에서 랜덤 선택
            int skillID = availableMidSkills[Random.Range(0, availableMidSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.lowSkillChance + table.midSkillChance + table.highSkillChance && availableHighSkills.Count > 0)
        {
            // highSkill 중에서 랜덤 선택
            int skillID = availableHighSkills[Random.Range(0, availableHighSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }

        // 뽕맛 골드 드랍 체크
        if (Random.value <= table.rareGoldChance)
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
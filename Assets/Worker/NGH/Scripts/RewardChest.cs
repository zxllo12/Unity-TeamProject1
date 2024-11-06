using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardChest : MonoBehaviour
{
    public int dataIndex; // ����� ���̺� �ε���
    public GameObject hpItemPrefab, skillPrefab, goldPrefab, rareGoldPrefab;

    public bool isOpened;
    [SerializeField] Animator animator;
    [SerializeField] float chestOpenDelay = 1f;

    [SerializeField] GameObject gate;

    private void Awake()
    {
        isOpened = false;
        GameManager.Instance.SetRewardChest(gameObject);
        gameObject.SetActive(false);
    }

    public void OpenChest()
    {
        if (isOpened) return; // �̹� ���� ���ڴ� �ٽ� ������ �ʵ���
        isOpened = true;

        Debug.Log("���� ����");

        // ���� ���� �ִϸ��̼� ���
        animator.SetTrigger("Open");

        SoundManager.Instance.Play(Enums.ESoundType.SFX, "OpenChest");

        // ���� �ð� �Ŀ� ������ �����ϴ� �ڷ�ƾ ����
        StartCoroutine(GenerateRewardsAfterDelay());
    }

    private IEnumerator GenerateRewardsAfterDelay()
    {
        Debug.Log("���� ������");

        // chestOpenDelay��ŭ ��ٸ� �� ������ ����
        yield return new WaitForSeconds(chestOpenDelay);

        // ���������� ��ϵ� �������̺��� �ε��ؼ� ������ ����
        DropData selectedTable = DataManager.Instance.DropDict[dataIndex];
        GenerateRewards(selectedTable);

        // ���� ���� �� ���� ��Ȱ��ȭ
        gameObject.SetActive(false);

        gate.SetActive(true);
    }

    private void GenerateRewards(DropData table)
    {
        // ��� ����
        int goldAmount = Random.Range(table.MinGold, table.MaxGold);
        Vector3 goldPosition = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, 0);
        GameObject goldDrop = Instantiate(goldPrefab, goldPosition, Quaternion.identity);
        goldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, goldAmount);

        // HP�� ��� ���� Ȯ��
        if (table.IsDropHP)
        {
            Vector3 potionPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
            GameObject potionDrop = Instantiate(hpItemPrefab, potionPosition, Quaternion.identity);
            potionDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Potion,0);
        }

        List<int> availableLowSkills = new List<int>();
        List<int> availableMidSkills = new List<int>();
        List<int> availableHighSkills = new List<int>();

        // �رݵ� ��ų���� ������� �ݺ�
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

        // ��ų ID ����Ʈ (0~8����)���� QWER�� ��ϵ��� ���� ��ų�� �����ϱ� ���� ����� �����մϴ�.
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

        // ��ų ��� Ȯ�� üũ
        float skillRoll = Random.value;
        if (skillRoll <= table.LowGradePercent && availableLowSkills.Count > 0)
        {
            // lowSkill �߿��� ���� ����
            int skillID = availableLowSkills[Random.Range(0, availableLowSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent && availableMidSkills.Count > 0)
        {
            // midSkill �߿��� ���� ����
            int skillID = availableMidSkills[Random.Range(0, availableMidSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }
        else if (skillRoll <= table.LowGradePercent + table.MidGradePercent + table.HighGradePercent && availableHighSkills.Count > 0)
        {
            // highSkill �߿��� ���� ����
            int skillID = availableHighSkills[Random.Range(0, availableHighSkills.Count)];
            CreateSkill(DropItem.ItemType.Skill, skillID);
        }

        // �͸� ��� ��� üũ
        if (Random.value <= table.BonusGoldPercent)
        {
            int bounsGoldAmount = Random.Range(table.MinGold, table.MaxGold) * 10;
            Vector3 bonusGoldPosition = new Vector3(transform.position.x + 2f, transform.position.y + 0.5f, 0);
            GameObject bonusGoldDrop = Instantiate(rareGoldPrefab, bonusGoldPosition, Quaternion.identity);
            bonusGoldDrop.GetComponent<DropItem>().Initialize(DropItem.ItemType.Gold, bounsGoldAmount);
        }
    }

    private void CreateSkill(DropItem.ItemType skillType, int skillID)
    {
        Vector3 skillPosition = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, 0);
        GameObject skillDrop = Instantiate(skillPrefab, skillPosition, Quaternion.identity);
        skillDrop.GetComponent<DropItem>().Initialize(skillType, skillID);
    }
}
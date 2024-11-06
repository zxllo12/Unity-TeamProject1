using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class PlayerSaveManager
{
    public static void SavePlayerSattus(int upgradedAtk, int upgradedDef, int upgradedHealth, int upgradedCooldown)
    {
        PlayerPrefs.SetInt("UpgradedAtk", upgradedAtk);
        PlayerPrefs.SetInt("UpgradedDef", upgradedDef);
        PlayerPrefs.SetInt("UpgradedHealth", upgradedHealth);
        PlayerPrefs.SetInt("UpgradedCooldown", upgradedCooldown);

        PlayerPrefs.Save();
    }

    public static void LoadPlayerStatus(out int upgradedAtk, out int upgradedDef, out int upgradedHealth, out int upgradedCooldown) 
    {
        upgradedAtk = PlayerPrefs.GetInt("UpgradedAtk", 0);
        upgradedDef = PlayerPrefs.GetInt("UpgradedDef", 0);
        upgradedHealth = PlayerPrefs.GetInt("UpgradedHealth", 0);
        upgradedCooldown = PlayerPrefs.GetInt("UpgradedCooldown", 0);
    }

    public static void SaveGold(int gold)
    {
        PlayerPrefs.SetInt("Gold", gold);

        PlayerPrefs.Save();
    }

    public static void LoadGold(out int gold)
    {
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public static void SaveUnlockedSkillList(string key, List<int> list)
    {
        // List�� �޸��� ���е� ���ڿ��� ��ȯ
        string listString = string.Join(",", list);

        // PlayerPrefs�� ���ڿ��� ����
        PlayerPrefs.SetString(key, listString);
        PlayerPrefs.Save();
        Debug.Log("UnlockedSkillList �����");
    }

    public static List<int> LoadUnlockedSkillList(string key)
    {
        // PlayerPrefs���� ���ڿ� �ҷ�����
        string listString = PlayerPrefs.GetString(key, string.Empty);

        // ����� �����Ͱ� ������ �� ����Ʈ ��ȯ
        if (string.IsNullOrEmpty(listString))
        {
            Debug.Log("�� UnlockedSkillList �ҷ���");
            return new List<int>();
        }

        // �޸��� ���ڿ��� �и��ϰ� int ����Ʈ�� ��ȯ
        string[] stringArray = listString.Split(',');
        List<int> intlist = new List<int>();

        foreach (string str in stringArray)
        {
            if(int.TryParse(str, out int value))
            {
                intlist.Add(value);
            }
        }
        Debug.Log("UnlockedSkillList �ҷ���");
        return intlist;
    }
}

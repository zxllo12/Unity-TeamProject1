using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockManager : MonoBehaviour
{
    public static SkillUnlockManager Instance;
    public List<int> unlockedSkills = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public bool IsSkillUnlocked(int skillID)
    {
        return unlockedSkills.Contains(skillID);
    }

    public void UnlockSkill(int skillID)
    {
        if (!unlockedSkills.Contains(skillID))
        {
            unlockedSkills.Add(skillID);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable
{
    public int minGold;
    public int maxGold;
    public bool hasHPItem;
    public float lowSkillChance;
    public float midSkillChance;
    public float highSkillChance;
    public float rareGoldChance;

    public DropTable(int minGold, int maxGold, bool hasHPItem, float lowSkillChance, float midSkillChance, float highSkillChance, float rareGoldChance)
    {
        this.minGold = minGold;
        this.maxGold = maxGold;
        this.hasHPItem = hasHPItem;
        this.lowSkillChance = lowSkillChance;
        this.midSkillChance = midSkillChance;
        this.highSkillChance = highSkillChance;
        this.rareGoldChance = rareGoldChance;
    }
}
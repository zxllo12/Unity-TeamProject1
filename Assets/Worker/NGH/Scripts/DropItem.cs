using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    public enum ItemType { Skill, Potion, Gold }
    public ItemType type;
    public Image image;
    public int goldAmount;
    public int skillID;

    public void Initialize(ItemType itemType, int value)
    {
        this.type = itemType;
        switch (itemType)
        {
            case ItemType.Skill:
                skillID = value;
                image.sprite = DataManager.Instance.SkillDict[skillID].SkillIcon;
                break;
            case ItemType.Gold:
                goldAmount = value;
                break;
            case ItemType.Potion:
                break;
        }
    }
}
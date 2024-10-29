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

    public void Initialize(ItemType itemType, int skillID, int gold = 0 )
    {
        type = itemType;
        goldAmount = gold;
        this.skillID = skillID;
        // 추가 설정 로직
    }
}
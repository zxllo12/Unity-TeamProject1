using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum SkillType
    {
        Basic,      // �⺻
        Fire,       // ��
        Water,      // ��
        Thunder,    // ����
        Wind        // �ٶ�
    }

    public enum Grade
    {
        Basic,      // �⺻
        Low,        // �ϱ�
        Mid,        // �߱�
        High        // ���
    }

    public enum PlayerSkillSlot
    {
        Slot1,
        Slot2,
        Slot3,
        Slot4,
        Length,
    }

    public enum ESoundType
    {
        BGM,
        SFX,
        Length,
    }
}

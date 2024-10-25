using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    SkillHandler handler;
    [SerializeField] Transform firePos;

    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];

    private void Awake()
    {
        handler = GetComponent<SkillHandler>();
    }

    private void Start()
    {
        handler.EquipSkill(0, Enums.PlayerSkillSlot.Slot1);
    }

    private void Update()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                handler.DoSkill((Enums.PlayerSkillSlot)i, firePos.position);
            }
        }
    }
}

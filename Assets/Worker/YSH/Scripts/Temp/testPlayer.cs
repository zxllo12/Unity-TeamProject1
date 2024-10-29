using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    SkillHandler handler;
    [SerializeField] Transform firePos;

    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];

    [SerializeField] float _attackPoint;

    private void Awake()
    {
        handler = GetComponent<SkillHandler>();
    }

    private void Start()
    {
        DataManager.Instance.OnLoadCompleted += testInit;
    }

    public void testInit()
    {
        handler.EquipSkill(0, Enums.PlayerSkillSlot.Slot1);
        handler.EquipSkill(1, Enums.PlayerSkillSlot.Slot2);
    }

    private void Update()
    {
        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                handler.DoSkill((Enums.PlayerSkillSlot)i, firePos.transform, _attackPoint);
            }
        }
    }

    private void OnDisable()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnLoadCompleted += testInit;
        }
    }
}

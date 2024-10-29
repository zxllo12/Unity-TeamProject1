using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    // 배틀쪽 UI 요소
    [Header("Skill Icon")]
    public Image[] skillimage = new Image[(int)Enums.PlayerSkillSlot.Length];

    private void Start()
    {
        GameManager.Instance.player.handler.OnChangedSkillSlot += UpdateSkill;
    }

    private void OnDisable()
    {
        GameManager.Instance.player.handler.OnChangedSkillSlot -= UpdateSkill;
    }

    public void UpdateSkill()
    {
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] == null)
            {
                skillimage[i].gameObject.SetActive(false);
            }
            else
            {
                skillimage[i].sprite = GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.SkillIcon;
                skillimage[i].gameObject.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    // 배틀쪽 UI 요소
    [Header("Skill Icon")]
    public Image[] skillimage = new Image[(int)Enums.PlayerSkillSlot.Length];
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI goldText;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // hpBar초기화
        hpBar.minValue = 0;
        hpBar.maxValue = GameManager.Instance.player.stats.maxHealth;

        // 이벤트 등록
        GameManager.Instance.player.handler.OnChangedSkillSlot += UpdateSkill;
        GameManager.Instance.OnGoldChanged += UpdateGold;
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        // gold UI TEST Code
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.AddGold(100);
        }
    }

    private void OnDisable()
    {
        // 이벤트 해제
        GameManager.Instance.player.handler.OnChangedSkillSlot -= UpdateSkill;
        GameManager.Instance.OnGoldChanged -= UpdateGold;
    }

    // 전투중 QWER스킬중 하나가 바뀌었을때 작동되는 코드
    public void UpdateSkill()
    {
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            // 빈 곳이 있으면 null
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] == null)
            {
                skillimage[i].gameObject.SetActive(false);
            }
            // 스킬이 슬롯에 있으면 그 스킬 데이터를 읽어서 스킬에 맞는 아이콘으로 교체
            else
            {
                skillimage[i].sprite = GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.SkillIcon;
                skillimage[i].gameObject.SetActive(true);
            }
        }
    }

    public void UpdateHp()
    {
        // 현재 체력과 최대 체력을 가져와서 UI를 업데이트
        float currentHealth = GameManager.Instance.player.stats.currentHealth;
        float maxHealth = GameManager.Instance.player.stats.maxHealth;

        hpBar.value = currentHealth;
        hpText.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateGold()
    {
        int gold = GameManager.Instance.GetGold();

        goldText.text = gold.ToString();
    }
}

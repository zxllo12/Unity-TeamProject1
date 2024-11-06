using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    // ��Ʋ�� UI ���
    [Header("Skill Icon")]
    public Image[] skillimage = new Image[(int)Enums.PlayerSkillSlot.Length];
    public Image[] cooldownImages = new Image[(int)Enums.PlayerSkillSlot.Length]; // ��Ÿ���� ���� �̹��� �迭
    public TextMeshProUGUI[] cooldownTexts = new TextMeshProUGUI[(int)Enums.PlayerSkillSlot.Length]; // ��Ÿ�� �ؽ�Ʈ �迭
    public GameObject GameOver;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI goldText;
    public SkillSlotUI skillSlotUI;

    private void Awake()
    {
        skillSlotUI.gameObject.SetActive(false);
        GameManager.Instance.SetBattleUI(this);
    }

    private void Start()
    {
        // hpBar�ʱ�ȭ
        hpBar.minValue = 0;
        hpBar.maxValue = GameManager.Instance.player.stats.maxHealth;

        // �̺�Ʈ ���
        GameManager.Instance.player.handler.OnChangedSkillSlot += UpdateSkill;
        GameManager.Instance.player.stats.OnChangedHP += UpdateHp;
        GameManager.Instance.OnGoldChanged += UpdateGold;
        GameManager.Instance.player.handler.OnSkillUsed += StartCooldown;

        UpdateGold();
        UpdateHp();
        UpdateSkill();
    }

    private void Update()
    {
        // gold UI TEST Code
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.AddGold(1000);
        }
    }

    private void OnDestroy()
    {
        Debug.Log($"BattleUI �ı��� {gameObject.name}");
        // �̺�Ʈ ����
        if(GameManager.Instance.player != null)
        {
            GameManager.Instance.player.handler.OnChangedSkillSlot -= UpdateSkill;
            GameManager.Instance.player.stats.OnChangedHP -= UpdateHp;
            GameManager.Instance.player.handler.OnSkillUsed -= StartCooldown; // ��Ÿ�� �̺�Ʈ ����
        }
        GameManager.Instance.OnGoldChanged -= UpdateGold;
    }

    public void ShowSkillSlotUI(int skillID)
    {
        skillSlotUI.SetInfo(skillID);
        skillSlotUI.gameObject.SetActive(true);
    }

    // ������ QWER��ų�� �ϳ��� �ٲ������ �۵��Ǵ� �ڵ�
    public void UpdateSkill()
    {
        for (int i = 0; i < (int)Enums.PlayerSkillSlot.Length; i++)
        {
            // �� ���� ������ null
            if (GameManager.Instance.player.handler.PlayerSkillSlot[i] == null)
            {
                skillimage[i].gameObject.SetActive(false);
                cooldownImages[i].fillAmount = 0; // �ʱ�ȭ
                cooldownImages[i].gameObject.SetActive(false); // ��Ÿ�� �̹��� ��Ȱ��ȭ
                cooldownTexts[i].gameObject.SetActive(false); // ��Ÿ�� �ؽ�Ʈ ��Ȱ��ȭ
            }
            // ��ų�� ���Կ� ������ �� ��ų �����͸� �о ��ų�� �´� ���������� ��ü
            else
            {
                skillimage[i].sprite = GameManager.Instance.player.handler.PlayerSkillSlot[i].SkillData.SkillIcon;
                skillimage[i].gameObject.SetActive(true);
                cooldownImages[i].gameObject.SetActive(true); // ��Ÿ�� �̹��� Ȱ��ȭ
            }
        }
    }

    public void UpdateHp()
    {
        // ���� ü�°� �ִ� ü���� �����ͼ� UI�� ������Ʈ
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

    private void StartCooldown(int skillIndex, float cooldownTime)
    {
        StartCoroutine(CooldownCoroutine(skillIndex, cooldownTime));
    }

    private IEnumerator CooldownCoroutine(int skillIndex, float cooldownTime)
    {
        float elapsed = 0f;
        cooldownTexts[skillIndex].gameObject.SetActive(true); // ��Ÿ�� �ؽ�Ʈ Ȱ��ȭ

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownImages[skillIndex].fillAmount = elapsed / cooldownTime; // ��Ÿ�� ���� ������Ʈ

            // ���� �ð� ����Ͽ� �Ҽ��� 1�ڸ��� ǥ��
            float remainingTime = cooldownTime - elapsed;
            cooldownTexts[skillIndex].text = remainingTime.ToString("F1");

            yield return null;
        }

        cooldownImages[skillIndex].fillAmount = 0; // ��Ÿ�� �Ϸ�
        cooldownTexts[skillIndex].gameObject.SetActive(false); // ��Ÿ�� ���� �� �ؽ�Ʈ ���� ó��
    }
}

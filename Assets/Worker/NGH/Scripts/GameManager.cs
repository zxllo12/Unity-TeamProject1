using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameManager Instance;

    // �÷��̾��� ���
    [SerializeField] private int gold;

    // ��� ��ȭ�� ������ �۵��ϴ� �̺�Ʈ
    public UnityAction OnGoldChanged;

    int?[] playerSkillSlotID = new int?[(int)Enums.PlayerSkillSlot.Length];
    public int?[] PlayerSkillSlotID { get { return playerSkillSlotID; } }

    public Player_Controller player;
    public BattleUI battleUI;
    GameObject rewardChest;
    public int monsterCount = 0;
    public int triggerCount = 0;

    public float battlePlayerMaxHP;
    public float battlePlayerAtk;
    public float battlePlayerDef;
    public float battlePlayerCurrentHP;
    public float skillCooltimeReduce;
    public bool playerCurHpSetOnce = true;

    private void Awake()
    {
        //�̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerSaveManager.LoadGold(out gold);
    }

    private void Update()
    {
        // �׽�Ʈ�� ��Ʋ �� ��ȯ
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestGame();
        }
    }

    private void OnEnable()
    {
        // ���� �ε�Ǹ� ȣ��� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ �Ǹ� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Debug.Log("GameManager �ı���");
        PlayerSaveManager.SaveGold(gold);
    }

    // ���� ���� �� Ÿ��Ʋ ȭ�鿡�� ȣ��
    public void StartGame()
    {
        // ���� �ε� ����
        StartCoroutine(LoadMainGame());
    }

    // ���� ���� �񵿱� �ε�
    private IEnumerator LoadMainGame()
    {
        // ���� ���� ���� �񵿱�� �ε�
        AsyncOperation operation = SceneManager.LoadSceneAsync("Stage1-1");

        // �ε� ���� ��Ȳ ������Ʈ
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // �׽�Ʈ �� ��ȯ�� �ڵ�
    public void TestGame()
    {
        // ���� �ε� ����
        StartCoroutine(LoadTestGame());
    }

    // �׽�Ʈ�� ������ ��ȯ
    private IEnumerator LoadTestGame()
    {
        // ���� ���� ���� �񵿱�� �ε�
        AsyncOperation operation = SceneManager.LoadSceneAsync("BattleTest");

        // �ε� ���� ��Ȳ ������Ʈ
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // ���� ������ �Ѿ�� ���� �ڵ�
    public void LoadScene(string sceneName)
    {
        // ���� �ε� ����
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    // ���� �������� �񵿱� �ε�
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // ���� ���������� �񵿱�� �ε�
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // �ε� ���� ��Ȳ ������Ʈ
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // �κ� ȭ������ ��ȯ
    public void ReturnToLobby()
    {
        // �κ� �ε� ����
        StartCoroutine(LoadLobby());
    }

    // �κ� �� �񵿱� �ε�
    private IEnumerator LoadLobby()
    {
        // �κ� ���� �񵿱� �ε�
        AsyncOperation operation = SceneManager.LoadSceneAsync("LobbyTest");

        // �ε� ���� ��Ȳ ������Ʈ
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // ���� �ε������ ȣ��Ǵ� �Լ�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded : " + scene.name);

        if(scene.name == "LobbyTest")
        {
            InitializeLobbyScene();
            return;
        }

        // �� �ε��� �ʱ�ȭ
        InitializeBattleScene();
        
    }

    public void SetPlayer(Player_Controller player)
    {
        this.player = player;
        SetPlayerStatus();
        if (playerCurHpSetOnce)
        {
            player.stats.currentHealth = player.stats.maxHealth;
            SetPlayerCurrentHP();
            playerCurHpSetOnce = false;
        }
        else
        {
            player.stats.currentHealth = battlePlayerCurrentHP;
        }
        player.stats.Dead += GameOver;
        player.stats.OnChangedHP += SetPlayerCurrentHP;

        // ���� ���� �ʿ� (�����Ϳ��� �⺻��ų ã����)
        player.handler.SetBasicSkill(9);

        for (int i = 0; i < playerSkillSlotID.Length; i++)
        {
            if (playerSkillSlotID[i] != null)
            {
                player.handler.EquipSkill((int)playerSkillSlotID[i], (Enums.PlayerSkillSlot)i);
            }
        }
    }

    public void SetRewardChest(GameObject rewardChest)
    {
        this.rewardChest = rewardChest;
    }

    public void SetBattleUI(BattleUI battleUI)
    {
        this.battleUI = battleUI;
    }

    public void SetMonster(MonsterState monster)
    {
        monsterCount++;
        monster.OnDead += DecreaseMonster;
    }

    public void DecreaseMonster(MonsterState monster)
    {
        monsterCount--;
        Debug.Log($"{monster.gameObject.name} ���");
        monster.OnDead -= DecreaseMonster;
        if (triggerCount == 0 && monsterCount == 0 && rewardChest != null)
        {
            rewardChest.SetActive(true);
            rewardChest = null;
        }
    }

    public void SetTrigger(Trigger trigger)
    {
        triggerCount++;
        trigger.triggerDestroyed += DecreaseTrigger;
    }

    public void DecreaseTrigger(Trigger trigger)
    {
        triggerCount--;
        trigger.triggerDestroyed -= DecreaseTrigger;
        if (triggerCount == 0 && monsterCount == 0 && rewardChest != null)
        {
            rewardChest.SetActive(true);
            rewardChest = null;
        }
    }

    // �� ��ȯ�� ���� �ʱ�ȭ
    private void InitializeBattleScene()
    {
        
    }

    private void InitializeLobbyScene()
    {
        if (player != null)
        {
            ResetPlayerStatus();
            player.stats.Dead -= GameOver;
            player.stats.OnChangedHP -= SetPlayerCurrentHP;
        }
        playerCurHpSetOnce = true;
        Time.timeScale = 1f;
        triggerCount = 0;
        monsterCount = 0;
        playerSkillSlotID = new int?[(int)Enums.PlayerSkillSlot.Length];
    }

    // ��带 �߰��ϴ� �޼���
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("��� �߰�: " + amount + ", ���� ���: " + gold);

        OnGoldChanged?.Invoke();
    }

    // ��带 �����ϴ� �޼���
    public void SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            gold -= amount;
            Debug.Log("��� ����: " + amount + ", ���� ���: " + gold);

            OnGoldChanged?.Invoke();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    // ��尡 ������� Ȯ���ϴ� �޼���
    public bool HasEnoughGold(int amount)
    {
        return gold >= amount;
    }

    // ���� ��� ������ �������� �޼��� (�ʿ� �� �߰�)
    public int GetGold()
    {
        return gold;
    }

    private void SetPlayerStatus()
    {
        player.stats.maxHealth = battlePlayerMaxHP;
        player.stats.attackPower = battlePlayerAtk;
        player.stats.defense = battlePlayerDef;
    }

    private void ResetPlayerStatus()
    {
        player.stats.maxHealth = 100;
        player.stats.attackPower = 10f;
        player.stats.defense = 5f;
        skillCooltimeReduce = 0f;
    }

    private void SetPlayerCurrentHP()
    {
        battlePlayerCurrentHP = player.stats.currentHealth;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(3);

        battleUI.GameOver.SetActive(true);

        yield return new WaitForSeconds(3);

        ReturnToLobby();
    }
}

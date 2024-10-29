using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class UIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UIManager Instance;

    // 로비 UI 요소
    [Header("Lobby UI")]
    public GameObject lobbyScreen;
    public Button magicShopButton;
    public Button statusUpgradeShopButton;
    public Button gameStartButton;

    // 마법 상점 UI 요소
    [Header("MagicShop UI")]
    public GameObject magicShopScreen;
    public Button magicShopExit;
    public Button[] skillButtons; // 상점 UI에 있는 각 스킬 버튼 배열
    public int[] skillCosts;      // 각 스킬의 해금 비용

    // 스테이터스 업그레이드 상점 UI 요소
    [Header("StatusUpgradeShop UI")]
    public GameObject statusUpgradeShopScreen;
    public Button statusUpgradeShopExit;


    private void Awake()
    {
        //싱글톤 패턴 구현
        if(Instance == null)
        {
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int skillID = i;
            skillButtons[i].onClick.AddListener(() => UnlockSkillInShop(skillID));
        }
    }

    private void UnlockSkillInShop(int skillID)
    {
        int cost = skillCosts[skillID];

        if (GameManager.Instance.HasEnoughGold(cost))
        {
            GameManager.Instance.SpendGold(cost);
            SkillUnlockManager.Instance.UnlockSkill(skillID);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void LinkButton()
    {
        // 버튼에 메서드 연결
        if (magicShopButton != null)
            magicShopButton.onClick.AddListener(EnterMagicShop);

        if (statusUpgradeShopButton != null)
            statusUpgradeShopButton.onClick.AddListener(EnterStatusShop);

        if (magicShopExit != null)
            magicShopExit.onClick.AddListener(ExitMagicShop);

        if (statusUpgradeShopExit != null)
            statusUpgradeShopExit.onClick.AddListener(ExitStatusShop);

        if (gameStartButton != null)
            gameStartButton.onClick.AddListener(GameManager.Instance.StartGame);
    }

    // 마법 상점 열기
    public void EnterMagicShop()
    {
        lobbyScreen?.SetActive(false);
        magicShopScreen?.SetActive(true);
    }

    // 마법 상점 닫기
    public void ExitMagicShop()
    {
        lobbyScreen?.SetActive(true);
        magicShopScreen?.SetActive(false);
    }

    // 스테이터스 상점 열기
    public void EnterStatusShop()
    {
        lobbyScreen?.SetActive(false);
        statusUpgradeShopScreen?.SetActive(true);
        
    }

    // 스테이터스 상점 닫기
    public void ExitStatusShop()
    {
        lobbyScreen?.SetActive(true);
        statusUpgradeShopScreen?.SetActive(false);
    }

    private void OpenOptionWindow()
    {
        
    }

    private void CloseOptionWindow()
    {

    }
}

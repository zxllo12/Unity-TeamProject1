using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UIManager Instance;

    // 로비 UI 요소
    [Header("Lobby UI")]
    public GameObject lobbyScreen;


    // 마법 상점 UI 요소
    [Header("MagicShop UI")]
    public GameObject magicShopScreen;

    // 스테이터스 업그레이드 상점 UI 요소
    [Header("StatusUpgradeShop UI")]
    public GameObject statusUpgradeShopScreen;

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

    // 마법 상점 열기
    public void EnterMagicShop()
    {
        if(lobbyScreen != null)
        {
            lobbyScreen.SetActive(false);
        }

        if(magicShopScreen != null)
        {
            magicShopScreen.SetActive(true);
        }
    }

    // 마법 상점 닫기
    public void ExitMagicShop()
    {
        if (lobbyScreen != null)
        {
            lobbyScreen.SetActive(true);
        }

        if (magicShopScreen != null)
        {
            magicShopScreen.SetActive(false);
        }
    }

    // 스테이터스 상점 열기
    public void EnterStatusShop()
    {
        if (lobbyScreen != null)
        {
            lobbyScreen.SetActive(false);
        }

        if (magicShopScreen != null)
        {
            statusUpgradeShopScreen.SetActive(true);
        }
    }

    // 스테이터스 상점 닫기
    public void ExitStatusShop()
    {
        if (lobbyScreen != null)
        {
            lobbyScreen.SetActive(true);
        }

        if (magicShopScreen != null)
        {
            statusUpgradeShopScreen.SetActive(false);
        }
    }

    public void EnterBattleScene()
    {

    }

    private void OpenOptionWindow()
    {

    }

    private void CloseOptionWindow()
    {

    }
}

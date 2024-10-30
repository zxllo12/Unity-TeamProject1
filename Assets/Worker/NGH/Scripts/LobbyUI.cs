using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class LobbyUI : MonoBehaviour
{
    public GameObject magicShop;
    public GameObject statusUpgradeShop;

    public Button StartButton;

    private void OnEnable()
    {
        LinkButton();
    }

    public void LinkButton()
    {
        if(StartButton != null)
        {
            StartButton.onClick.AddListener(GameManager.Instance.StartGame);
        }
    }

    // 마법 상점 열기
    public void EnterMagicShop()
    {
        gameObject?.SetActive(false);
        magicShop?.SetActive(true);
    }

    // 마법 상점 닫기
    public void ExitMagicShop()
    {
        gameObject?.SetActive(true);
        magicShop?.SetActive(false);
    }

    // 스테이터스 상점 열기
    public void EnterStatusShop()
    {
        gameObject?.SetActive(false);
        statusUpgradeShop?.SetActive(true);
        
    }

    // 스테이터스 상점 닫기
    public void ExitStatusShop()
    {
        gameObject?.SetActive(true);
        statusUpgradeShop?.SetActive(false);
    }

    private void OpenOptionWindow()
    {
        
    }

    private void CloseOptionWindow()
    {

    }
}

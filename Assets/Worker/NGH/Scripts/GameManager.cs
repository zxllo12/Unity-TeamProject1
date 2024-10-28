using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance;

    private void Awake()
    {
        //싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReturnToLobby();
        }
    }

    // 게임 시작 시 타이틀 화면에서 호출
    public void StartGame()
    {
        // 게임 로딩 시작
        StartCoroutine(LoadMainGame());
    }

    // 메인 게임 비동기 로딩
    private IEnumerator LoadMainGame()
    {
        // 메인 게임 씬을 비동기로 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync("BattleTest");

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // 로비 화면으로 귀환
    public void ReturnToLobby()
    {
        // 로비 로딩 시작
        StartCoroutine(LoadLobby());
    }

    // 로비 씬 비동기 로딩
    private IEnumerator LoadLobby()
    {
        // 로비 씬을 비동기 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync("LobbyTest");

        // 로딩 진행 상황 업데이트
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}

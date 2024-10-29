using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        // 테스트용 로비 전환
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReturnToLobby();
        }
    }

    private void OnEnable()
    {
        // 씬이 로드되면 호출될 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 비활성화 되면 이벤트 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    // 씬이 로드됐을때 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded : " + scene.name);

        // 씬 로드후 초기화
        if(scene.name == "BattleTest")
        {
            InitializeBattleScene();
        }

        if(scene.name == "LobbyTest")
        {
            InitializeLobbyScene();
        }
    }

    // 씬 전환후 연결 초기화
    private void InitializeBattleScene()
    {
        UIManager.Instance.lobbyScreen = null;
        UIManager.Instance.magicShopScreen = null;
        UIManager.Instance.statusUpgradeShopScreen = null;
        UIManager.Instance.battleUI = GameObject.FindWithTag("Battle");
    }

    private void InitializeLobbyScene()
    {
        // 인스펙터 상 각 오브젝트 연결
        UIManager.Instance.lobbyScreen = GameObject.FindWithTag("Lobby");
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Magicshop"))
            {
                UIManager.Instance.magicShopScreen = obj;
            }
            if (obj.CompareTag("Statusshop"))
            {
                UIManager.Instance.statusUpgradeShopScreen = obj;
            }
        }
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in allButtons)
        {
            if(button.CompareTag("Gamestart"))
            {
                UIManager.Instance.gameStartButton = button;
            }
            if (button.CompareTag("Magicshopenter"))
            {
                UIManager.Instance.magicShopButton = button;
            }
            if(button.CompareTag("Statusshopenter"))
            {
                UIManager.Instance.statusUpgradeShopButton = button;
            }
            if(button.CompareTag("Magicshopexit"))
            {
                UIManager.Instance.magicShopExit = button;
            }
            if (button.CompareTag("Statusshopexit"))
            {
                UIManager.Instance.statusUpgradeShopExit = button;
            }
        }
        UIManager.Instance.LinkButton();
        UIManager.Instance.battleUI = null;
    }
}

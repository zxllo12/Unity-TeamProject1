using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public GameObject magicShop;
    public GameObject statusUpgradeShop;

    public Button StartButton;

    private void Start()
    {
        LinkButton();
    }

    public void LinkButton()
    {
        if (StartButton != null)
        {
            SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
            StartButton.onClick.AddListener(GameManager.Instance.StartGame);
        }
    }

    // ���� ���� ����
    public void EnterMagicShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(false);
        magicShop?.SetActive(true);
    }

    // ���� ���� �ݱ�
    public void ExitMagicShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(true);
        magicShop?.SetActive(false);
    }

    // �������ͽ� ���� ����
    public void EnterStatusShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
        gameObject?.SetActive(false);
        statusUpgradeShop?.SetActive(true);

    }

    // �������ͽ� ���� �ݱ�
    public void ExitStatusShop()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "PushButton");
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

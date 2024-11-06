using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private Image interactionIcon;  // ��ȣ�ۿ� ������ �̹���

    private void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);  // ���� �� �������� ��Ȱ��ȭ
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(true);  // Ʈ���� ���� �� ������ Ȱ��ȭ
            UpdateIconPosition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);  // Ʈ���� ��� �� ������ ��Ȱ��ȭ
        }
    }

    private void Update()
    {
        if (interactionIcon.gameObject.activeSelf)
        {
            UpdateIconPosition();  // �� �����Ӹ��� ������ ��ġ ������Ʈ
        }
    }

    private void UpdateIconPosition()
    {
        if (interactionIcon != null)
        {
            interactionIcon.transform.position = transform.position + new Vector3(0f, 1f, -2f);  // ������ ��ġ�� iconPosition ��ġ�� ����
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultHide : MonoBehaviour
{
    private void OnEnable()
    {
        // 2�� �� resultWindow�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
        StartCoroutine(HideResultWindowAfterDelay());
    }

    private IEnumerator HideResultWindowAfterDelay()
    {
        yield return new WaitForSeconds(2);  // 2�� ���
        gameObject.SetActive(false);       // 2�� �� resultWindow ��Ȱ��ȭ
    }
}

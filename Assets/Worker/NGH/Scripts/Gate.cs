using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] string nextScene;

    public void MoveNextScene()
    {
        GameManager.Instance.LoadScene(nextScene);
    }
}

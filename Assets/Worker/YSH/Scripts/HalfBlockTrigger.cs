using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBlockTrigger : MonoBehaviour
{
    [SerializeField] bool isDownTrigger;
    [SerializeField] Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        if (isDownTrigger)
        {
            _collider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isDownTrigger)
        {
            _collider.isTrigger = false;
        }
    }
}

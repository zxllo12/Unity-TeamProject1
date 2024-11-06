using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    BoxCollider AttackCollider;
    float damage;
    [SerializeField] float disableTime;
    private void Awake()
    {
       AttackCollider = GetComponent<BoxCollider>();
        AttackCollider.enabled = false;
    }

    public void TirggerOnOff()
    {
        Debug.Log("Ʈ���� Ȯ��");
        AttackCollider.enabled = true;
        StartCoroutine(DisableRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Ʈ���� �浹ü �̸�:{other.name}");
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.stats.TakeDamage(damage);
        }
    }

    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(disableTime);
        AttackCollider.enabled = false;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}

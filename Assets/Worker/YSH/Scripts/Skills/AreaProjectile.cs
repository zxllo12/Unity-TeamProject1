using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : Projectile
{
    BoxCollider _triggerCollider;

    ParticleSystem _particle;
    float _destroyTime;

    private void Awake()
    {
        _triggerCollider = GetComponent<BoxCollider>();
        _triggerCollider.enabled = false;

        _particle = GetComponent<ParticleSystem>();
        _destroyTime = _particle.main.duration;

        StartCoroutine(DestroyAreaProjectile());
    }

    public void EnableTrigger()
    {
        Debug.Log($"Enable Trigger : {gameObject.name}");
        _triggerCollider.enabled = true;
        //_triggerCollider.enabled = false;
    }

    IEnumerator DestroyAreaProjectile()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_destroyTime);
        yield return waitTime;
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} hit : {other.name}");

        if (hitEffect != null)
            Instantiate(hitEffect, other.transform.position, Quaternion.identity);

        MonsterState monster = other.GetComponent<MonsterState>();
        if (monster != null)
        {
            monster.IsHit(_damage);
        }

        Debug.Log($"Projectile Damage : {_damage}");
    }
}

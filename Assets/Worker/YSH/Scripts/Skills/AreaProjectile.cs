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

        if (_triggerCollider != null)
            _triggerCollider.enabled = false;

        _particle = GetComponent<ParticleSystem>();
        _destroyTime = _particle.main.duration;

        StartCoroutine(DestroyAreaProjectile());
    }

    public void EnableTrigger()
    {
        //Debug.Log($"Enable Trigger : {gameObject.name}");
        _triggerCollider.enabled = true;
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
        {
            ParticleSystem effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }

        MonsterState monster = other.GetComponent<MonsterState>();
        if (monster != null)
        {
            monster.IsHit(_damage);
        }

        Debug.Log($"Projectile Damage : {_damage}");
    }

    // 파티클 시스템의 Collision 이용 시 
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"{gameObject.name} hit : {other.name}");

        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }

        MonsterState monster = other.GetComponent<MonsterState>();
        if (monster != null)
        {
            monster.IsHit(_damage);
        }

        Debug.Log($"Projectile Damage : {_damage}");
    }
}

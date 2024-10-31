using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ParticleSystem flashEffect;
    [SerializeField] protected ParticleSystem hitEffect;

    protected Coroutine _moveRoutine;

    protected float _damage;

    protected SkillBase _ownerSkill;

    public virtual void SetDamage(float damage)
    {
        _damage = damage;
    }

    public virtual void Fire(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        _ownerSkill = skill;

        if (flashEffect != null)
        {
            ParticleSystem effect = Instantiate(flashEffect, startPos, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration);
        }

        _moveRoutine = StartCoroutine(MoveRoutine(skill, startPos, dir, speed));
    }

    protected IEnumerator MoveRoutine(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        while (true)
        {
            Vector3 moveDist = transform.position + dir * speed * Time.deltaTime;
            if ((moveDist - startPos).sqrMagnitude > skill.SkillData.Range * skill.SkillData.Range)
            {
                break;
            }

            transform.position += dir * speed * Time.deltaTime;

            yield return null;
        }

        _moveRoutine = null;
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
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

        // 관통 여부 확인
        if (_ownerSkill.SkillData.CanPenetration == false)
            Destroy(gameObject);
    }
}

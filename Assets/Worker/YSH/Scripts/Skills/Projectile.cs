using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ParticleSystem flashEffect;
    [SerializeField] ParticleSystem hitEffect;

    Coroutine _moveRoutine;

    float _damage;

    SkillBase _ownerSkill;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void Fire(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        _ownerSkill = skill;

        ParticleSystem flash = Instantiate(flashEffect, startPos, Quaternion.identity);

        _moveRoutine = StartCoroutine(MoveRoutine(skill, startPos, dir, speed));

        //StartCoroutine(DestroyRoutine(skill));
    }

    IEnumerator MoveRoutine(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
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

    //IEnumerator DestroyRoutine(SkillBase skill)
    //{
    //    WaitForSeconds _destroyTime = new WaitForSeconds(skill.SkillData.Range / skill.SkillData.ProjectileSpeed);

    //    yield return _destroyTime;

    //    StopCoroutine(_moveRoutine);
    //    _moveRoutine = null;

    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} hit : {other.name}");
        ParticleSystem hit = Instantiate(hitEffect, other.transform.position, Quaternion.identity);

        //MonsterState monster = other.GetComponent<MonsterState>();
        //if (monster != null)
        //{
        //    monster.IsHit();
        //}

        // 관통 여부 확인
        if (_ownerSkill.SkillData.CanPenetration == false)
            Destroy(gameObject);
    }
}

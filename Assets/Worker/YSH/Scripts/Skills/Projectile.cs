using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float destroyTime;

    Coroutine _moveRoutine;

    float _damage;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void Fire(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        _moveRoutine = StartCoroutine(MoveRoutine(skill, startPos, dir, speed));

        StartCoroutine(DestroyRoutine(skill));
    }

    IEnumerator MoveRoutine(SkillBase skill, Vector3 startPos, Vector3 dir, float speed)
    {
        while (true)
        {
            transform.position += dir * speed * Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator DestroyRoutine(SkillBase skill)
    {
        WaitForSeconds _destroyTime = new WaitForSeconds(skill.SkillData.Range / skill.SkillData.ProjectileSpeed);

        yield return _destroyTime;

        StopCoroutine(_moveRoutine);
        _moveRoutine = null;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Creature creature = other.GetComponent<Creature>();
        //if (creature != null)
        //{
        //    creature.TakeDamage(_damage);

        //    Destroy(gameObject);
        //}

        Debug.Log($"{gameObject.name} hit : {other.name}");
    }
}

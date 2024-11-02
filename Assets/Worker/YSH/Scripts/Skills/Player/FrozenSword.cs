using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenSword : SkillBase
{
    const int swordCount = 3;

    [SerializeField] float yInterval = 0.575f;
    [SerializeField] float xInterval = 2f;
    [SerializeField] Projectile[] SwordPrefab = new Projectile[swordCount];

    Coroutine _skillRoutine;
    [SerializeField] float[] DelayTimes = new float[swordCount];

    Vector3[] _positions = new Vector3[swordCount];

    public override void DoSkill(float attackPoint)
    {
        if (_skillRoutine != null)
            return;

        Debug.Log("IceArrow DoSkill");
        _skillRoutine = StartCoroutine(SkillRoutine(attackPoint));
    }

    IEnumerator SkillRoutine(float attackPoint)
    {
        base.DoSkill(attackPoint);

        _positions[0] = new Vector3(CastPos.x, CastPos.y - yInterval, CastPos.z);
        _positions[1] = new Vector3(CastPos.x - xInterval, CastPos.y - yInterval, CastPos.z);
        _positions[2] = new Vector3(CastPos.x + xInterval, CastPos.y - yInterval, CastPos.z);

        for (int i = 0; i < SwordPrefab.Length; i++)
        {
            AreaProjectile projectile = Instantiate(SwordPrefab[i], _positions[i], Quaternion.identity) as AreaProjectile;
            ParticleSystem.MainModule main = projectile.gameObject.GetComponent<ParticleSystem>().main;
            main.startDelay = DelayTimes[i];
            projectile.SetDamage(_skillData.Damage * attackPoint);
            projectile.EnableTrigger();

            yield return null;
        }

        _skillRoutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoom : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoCast()
    {
        base.DoCast();

        _castEffectInstance.transform.position = _castPos;

        _castEffectInstance.Play();
    }

    public override void DoSkill(float attackPoint)
    {
        AreaProjectile projectile = Instantiate(projectilePrefab, _castPos, Quaternion.identity) as AreaProjectile;
        projectile.SetDamage(_skillData.Damage * attackPoint);
        projectile.SetTriggerSize(_skillData.Radius);
        projectile.EnableTrigger();
        base.DoSkill(attackPoint);
    }
}

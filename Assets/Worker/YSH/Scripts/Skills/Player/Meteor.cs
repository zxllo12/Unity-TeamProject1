using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoCast()
    {
        base.DoCast();
    }

    public override void DoSkill(float attackPoint)
    {
        AreaProjectile projectile = Instantiate(projectilePrefab, _castPos, Quaternion.identity) as AreaProjectile;
        ParticleSystem.ShapeModule shape = projectile.GetComponent<ParticleSystem>().shape;
        shape.scale = new Vector3(_skillData.Radius, 1f, 1f);
        projectile.SetDamage(_skillData.Damage * attackPoint);
        base.DoSkill(attackPoint);
    }
}

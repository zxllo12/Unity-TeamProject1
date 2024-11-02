using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpear : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill(float attackPoint)
    {
        Projectile projectile = Instantiate(projectilePrefab, StartPos, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage * attackPoint);
        projectile.transform.Rotate(0, 0, -(90f * StartDir));
        projectile.Fire(this, StartPos, projectile.transform.up, _skillData.ProjectileSpeed);
        base.DoSkill(attackPoint);
    }
}

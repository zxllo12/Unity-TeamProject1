using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FireBall : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill(float attackPoint)
    {
        Projectile projectile = Instantiate(projectilePrefab, StartPos.position, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage * attackPoint);
        projectile.transform.rotation = Quaternion.LookRotation(_startPos.forward);
        projectile.Fire(this, _startPos.position, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill(attackPoint);
    }
}

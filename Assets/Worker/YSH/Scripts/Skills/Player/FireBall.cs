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

    public override void DoSkill()
    {
        Projectile projectile = Instantiate(projectilePrefab, StartPos, Quaternion.identity);
        projectile.SetDamage(_skillData.Damage);
        projectile.transform.rotation = Quaternion.LookRotation(_user.transform.right);
        projectile.Fire(this, _startPos, projectile.transform.forward, _skillData.ProjectileSpeed);
        base.DoSkill();
    }
}

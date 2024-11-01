using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : SkillBase
{
    public override void SetData(int id)
    {
        base.SetData(id);
    }

    public override void DoSkill(float attackPoint)
    {
        StartCoroutine(SkillRoutine(attackPoint));
    }

    IEnumerator SkillRoutine(float attackPoint)
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        int arrowCount = 3;

        float Yinterval = 0.2f;
        float yPos = StartPos.position.y + Yinterval;
        Vector3 initPos = new Vector3(StartPos.position.x, yPos, StartPos.position.z);

        for (int i = 0; i < arrowCount; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, initPos, Quaternion.identity);
            projectile.SetDamage(_skillData.Damage * attackPoint);
            projectile.transform.Rotate(0, 0, -(90f * _startPos.forward.x));
            projectile.Fire(this, initPos, projectile.transform.up, _skillData.ProjectileSpeed);
            initPos.y += -Yinterval;

            if (i < arrowCount-1)
                yield return waitTime;
        }
  
        base.DoSkill(attackPoint);
    }
}

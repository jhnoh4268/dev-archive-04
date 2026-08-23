using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class Lighting : MonsterSkill
{
    public float skillCount = 5.0f;
    public float radiusAroundPlayer = 5.0f;
    public float delayCircle = 0.3f;
    
    public float explosionRange = 2.0f;
    public float damage = 10.0f;

    public override IEnumerator CastSkill()
    {
        for(int i = 0; i < skillCount; i++)
        {
            Vector3 randomPos = monster.target.transform.position + UtilsWorld.GetRandomCircleOffset(radiusAroundPlayer);

            var circle = MANAGER.poolManager.PoolingObj("AttackEffectCircle").Get((value) =>
            {
                value.transform.position = randomPos;

            });

            var skill = circle.GetComponent<MonsterEffectCircle>();
            skill.Initialize(monster.target, explosionRange, damage);

            yield return new WaitForSeconds(delayCircle);
        }

        yield return new WaitForSeconds(1f);
    }
}

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill06_MultiSlash : SkillBase
{
    private int count;
    private float range;
    private Vector3 scale;
    protected override void Fire()
    {
        StartCoroutine(SwordDelay());
    }

    IEnumerator SwordDelay()
    {
        bool isRight = true;
        for(int i = 0; i < count; i++)
        {
            var sword = MANAGER.poolManager.PoolingObj("Sword").Get((value) =>
            {
                value.transform.position = 
                    (Player.instance.transform.position +
                    new Vector3(0.0f, 1.0f, 0f) +
                    Player.instance.transform.forward * 3.0f);

                float zRotation = isRight ? Random.Range(0f, 60f) : Random.Range(0f, -60f);
                isRight = !isRight;

                value.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
                value.transform.localScale = scale;

                MANAGER.instance.Run(UtilsCoroutine.Delay(1f, () =>
                {
                    MANAGER.poolManager.m_Pool_Dictionary["Sword"].Return(value);
                }));
            });

            // 부채꼴 방향 공격
            var targets = GetTargetsList(range);

            Vector3 origin = Player.instance.transform.position;
            Vector3 forward = Player.instance.transform.forward;
            List<Monster> monsterList = new();
            foreach(var target in targets)
            {
                if(target.TryGetComponent<Monster>(out Monster m) && m.isSpawned)
                {
                    Vector3 dirToTarget = (target.transform.position - origin).normalized;

                    float dot = Vector3.Dot(forward, dirToTarget);
                    // dot
                    // 1 -> 정면
                    // 0.7 ~ 0.9 -> 약간정면
                    // 0 -> 측면
                    // -1 -> 완전 반대
                    float threshold = Mathf.Cos(45.0f * 0.5f * Mathf.Deg2Rad);
                    // Mathf.Cos(22.5) -> 0.924f

                    if(dot >= threshold)
                    {
                        monsterList.Add(m);
                    }
                }
            }

            foreach(var monster in monsterList)
            {
                if(monster.isDead) continue;
                monster.GetDamage(Damage(monster.GetComponent<StatusEffect>()));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    protected override void OnInitalize()
    {
        count = 2 + (level-1);
        range = 5.0f + 1.0f * (level-1);
        float scalefloat = 1 + 0.25f * (level-1);
        scale = new Vector3 (scalefloat, 1, scalefloat);
    }

    protected override void OnLevelUp()
    {
        count = 2 + (level - 1);
        range = 5.0f + 1.0f * (level - 1);
        float scalefloat = 1 + 0.25f * (level - 1);
        scale = new Vector3(scalefloat, 1, scalefloat);
    }
}

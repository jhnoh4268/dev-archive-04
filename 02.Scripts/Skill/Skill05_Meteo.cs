using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill05_Meteo : SkillBase
{
    private int count;
    protected override void Fire()
    {
        StartCoroutine(MeteoCouroutine());
    }

    IEnumerator MeteoCouroutine()
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.2f);

            var laser = MANAGER.poolManager.PoolingObj("Meteo").Get((value) =>
            {
                Vector3 pos = RandomPos(10.0f);
                value.transform.position = pos;

                foreach(var monster in RangeMonsterList(pos, 5.0f))
                {
                    monster.GetDamage(Damage());
                    Effect_Status status = (Effect_Status)Random.Range(0, (int)Effect_Status.Stun);

                    MANAGER.SkillManager.ApplyStatus(status, monster.GetComponent<StatusEffect>(), 0.3f);
                }

                MANAGER.instance.Run(UtilsCoroutine.Delay(1f, () =>
                {
                    MANAGER.poolManager.m_Pool_Dictionary["Meteo"].Return(value);
                }));
            });
        }
    }

    protected override void OnInitalize()
    {
        count = (level);
    }

    protected override void OnLevelUp()
    {
        count = (level);
    }
}

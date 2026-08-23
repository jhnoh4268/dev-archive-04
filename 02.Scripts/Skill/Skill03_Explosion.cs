using UnityEngine;

public class Skill03_Explosion : SkillBase
{
    Vector3 particleRange;
    float targetRange;
    protected override void Fire()
    {
        var explosion = MANAGER.poolManager.PoolingObj("Explosion").Get((value) =>
        {
            value.transform.position = Player.instance.transform.position;
            value.transform.localScale = particleRange;

            var targetsList = GetTargetsList(targetRange);
            foreach(var hit in targetsList)
            {
                if(hit.GetComponent<Monster>().isSpawned)
                {
                    hit.GetComponent<Monster>().GetDamage(Damage(hit.GetComponent<StatusEffect>()));
                }
            }

            StartCoroutine(UtilsCoroutine.Delay(1.0f, () =>
            {
                MANAGER.poolManager.m_Pool_Dictionary["Explosion"].Return(value);
            }));
        });
    }

    protected override void OnInitalize()
    {
        float range = 1.0f + 0.3f * (level-1);
        particleRange = new Vector3(range, range, range);
        targetRange = 5.0f + 1.0f * (level-1);
    }

    protected override void OnLevelUp()
    {
        float range = 1.0f + 0.3f * (level - 1);
        particleRange = new Vector3(range, range, range);
        targetRange = 5.0f + 1.0f * (level - 1);
    }
}

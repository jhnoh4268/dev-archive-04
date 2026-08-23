using UnityEngine;

public class Skill04_BlueField : SkillBase
{
    Vector3 particleRange;
    float targetRange;

    private GameObject BlueFieldParticle;
    protected override void Fire()
    {
        var targetsList = GetTargetsList(targetRange);
        foreach(var hit in targetsList)
        {
            if(hit.GetComponent<Monster>().isSpawned)
            {
                hit.GetComponent<Monster>().GetDamage(Damage(hit.GetComponent<StatusEffect>()));
            }
        }
    }

    protected override void OnInitalize()
    {
        float range = 0.1f + 0.01f * (level - 1);
        particleRange = new Vector3(range, 0.1f, range); 
        targetRange = 4.0f + 1.0f * (level - 1);

        BlueFieldParticle = MANAGER.poolManager.PoolingObj("BlueField").Get((value) =>
        {
            value.transform.parent = Player.instance.transform;
            value.transform.localPosition = Vector3.zero;
            value.transform.localScale = particleRange;
        });
    }

    protected override void OnLevelUp()
    {
        float range = 0.1f + 0.01f * (level - 1);
        particleRange = new Vector3(range, 0.1f, range);

        if(BlueFieldParticle != null)
        {
            BlueFieldParticle.transform.localScale = particleRange;
        }
        targetRange = 4.0f + 1.0f * (level - 1);
    }
}

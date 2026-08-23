using System.Collections.Generic;
using UnityEngine;

public class Skill01_Lighting : SkillBase
{
    public List<GameObject> LightingObject = new List<GameObject>();
    protected override void OnInitalize() { }

    protected override void OnLevelUp() { }

    protected override void Fire()
    {

        for (int i = 0; i < level; i++)
        {
            List<Transform> targetsList = GetTargetsList(10.0f);
            if(targetsList.Count <= 0) return;
            
            Transform targetPoint = targetsList[Random.Range(0, targetsList.Count)];
            var lighting = MANAGER.poolManager.PoolingObj("Lighting").Get((value) =>
            {
                value.transform.position = targetPoint.position + new Vector3(0, 5.5f, 0);
                value.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                value.GetComponent<ParticleSystem>().Play();
                
                targetPoint.GetComponent<Monster>().GetDamage(Damage(
                    targetPoint.GetComponent<StatusEffect>()));
                
            });
        }
    }
}

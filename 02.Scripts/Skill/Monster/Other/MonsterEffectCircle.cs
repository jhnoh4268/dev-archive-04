using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterEffectCircle : MonoBehaviour
{
    public Transform target;
    private float explosionRange;
    private float damage;

    private float growDuration = 1.5f;
    private float timer = 0.0f;

    private bool exploded = false;

    public Transform circleRenderer;
    public Transform circleLine;
    public void Initialize(Transform player, float range, float dmg)
    {
        target = player;
        explosionRange = range;
        damage = dmg;
        exploded = false;
        timer = 0;

        float lineScale = range * 2.0f;
        circleLine.localScale = new Vector3(lineScale, lineScale);

        circleRenderer.localScale = Vector3.zero;

        StartCoroutine(nameof(GrowAndExplode));
    }

    IEnumerator GrowAndExplode()
    {
        while(timer < growDuration)
        {
            timer += Time.deltaTime;
            float t = timer / growDuration;

            float targetScale = circleLine.localScale.x;
            circleRenderer.localScale = Vector3.Lerp(Vector3.zero, new Vector3(targetScale, targetScale), t);
            Debug.Log(targetScale);

            yield return null;
        }

        Exploed();
    }

    void Exploed()
    {
        if(exploded) return;
        exploded = true;

        var lighting = MANAGER.poolManager.PoolingObj("BossSkill").Get((value)=>{
            value.transform.position = transform.position + new Vector3(0, 10.0f, 0);
            value.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            value.GetComponent<ParticleSystem>().Play();
        });

        float dist = Vector3.Distance(target.position, transform.position);
        if(dist < explosionRange)
        {
            target.GetComponent<Player>().GetDamage(damage);
        }

        MANAGER.poolManager.m_Pool_Dictionary["AttackEffectCircle"].Return(this.gameObject);
    }
}

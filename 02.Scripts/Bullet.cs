using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public List<StatusEffectClass> status = new();
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject bulletParticle;

    public string BulletName;

    public float moveSpeed = 10.0f;
    public float lifetime = 5.0f;
    public float delay;
    public float damage;

    private Vector3 direction;
    private bool isHit = false;


    public void Initalize(Vector3 dir, float dmg, string bulletName, List<StatusEffectClass> effectClass = null)
    {
        isHit = false;
        BulletName = bulletName;
        direction = dir;
        damage = dmg;
        status = effectClass;

        bulletParticle.SetActive(true);
        explosionParticle.SetActive(false);
        
        StartCoroutine(DestoryCoroutine(5f));
    }

    private void Update()
    {
        if(isHit) return;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator DestoryCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        MANAGER.poolManager.m_Pool_Dictionary[BulletName].Return(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isHit) return;

        if(other.gameObject.layer == LayerMask.NameToLayer("MONSTER"))
        {
            isHit = true;
            bulletParticle.SetActive(false);
            explosionParticle.SetActive(true);
            
            other.gameObject.GetComponent<Monster>().GetDamage(damage);

            if(status != null)
            {
                for(int i = 0; i < status.Count; i++)
                {
                    MANAGER.SkillManager.ApplyStatus(
                        status[i].status,
                        other.gameObject.GetComponent<StatusEffect>(),
                        status[i].value);
                }
                
            }

            StopAllCoroutines();
            StartCoroutine(WaitEffectAndReturn(delay));
        }
    }

    IEnumerator WaitEffectAndReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBullet();
    }

    private void ReturnBullet()
    {
        explosionParticle.SetActive(false);
        StopAllCoroutines();
        MANAGER.poolManager.m_Pool_Dictionary[BulletName].Return(gameObject);
    }
}

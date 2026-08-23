using UnityEngine;

public class ExplotionEffect : MonoBehaviour, IItemEffect
{
    public float radius = 8.0f;
    public float damage = 100.0f;

    public GameObject expEffect;

    private bool isPickUp = false;
    public void Initlize()
    {
        isPickUp = false;
    }

    public void OnPickUp(GameObject owner)
    {
        if(isPickUp) return;
        isPickUp = true;

        Vector3 center = transform.position;
        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("MONSTER"));
        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out Monster m) && m.isSpawned)
            {
                m.GetDamage(damage);
            }
        }
        
        Instantiate(expEffect, center, Quaternion.identity);
    }
}

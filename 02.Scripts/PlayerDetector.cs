using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public LayerMask orbLayer;

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Magnet(), orbLayer);

        foreach (Collider col in hits)
        {
            Orb orb = col.gameObject.GetComponent<Orb>();
            if (orb != null)
            {
                orb.StartFollow(transform);
            }
        }
    }

    private float Magnet()
    {
        float baseMagnet = MANAGER.Session.magnetRadius;
        float magnet = baseMagnet + baseMagnet * (MANAGER.Session.magnetRadiusPercentage / 100);

        return magnet;
    }
}
